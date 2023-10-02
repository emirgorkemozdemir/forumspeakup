using Azure.Core;
using BusinessLayer.Concrete;
using deviceInterface.ExtraFiles;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Voice_Form.Models;
using Voice_Form.Extra_Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Voice_Form.Controllers
{
    public class UserController : Controller
    {
        UserBL user_manager = new UserBL();
        private readonly IWebHostEnvironment _hostEnvironment;
        public UserController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            SessionOperations.KillAllSessions(this);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(TableUser new_user)
        {
            if (ModelState.IsValid)
            {
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
                new_user.UserIpAdress = remoteIpAddress.ToString();
                if (/*VpnChecker.CheckIP(remoteIpAddress.ToString()) == 0*/ false)
                {
                    ViewBag.validateError = "Vpn kullanımı tespit edildi. Vpn kapatıp kullanın.";
                    return View();
                }
                else
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var filePath = Path.Combine(wwwRootPath + "/extra/badwords.json");
                    var password2 = Request.Form["tboxPasswordControl"];
                    var existingUser = user_manager.UserRegisterExistingCheckBL(new_user);
                    if (existingUser != "")
                    {
                        ViewBag.validateError = existingUser;
                        return View();
                    }
                    else if (new_user.UserPassword != password2)
                    {
                        ViewBag.validateError = "Girdiğiniz şifreler eşleşmiyor.";
                        return View();
                    }
                    else if (BadWords.CheckBadWords(new_user.UserName, filePath))
                    {
                        ViewBag.validateError = "Küfürlü içerik tespit edildi";
                        return View();
                    }
                    else if (! user_manager.UserCheckIPAdress(new_user.UserIpAdress))
                    {
                        ViewBag.validateError = "Çok fazla hesap oluşturdunuz";
                        return View();
                    }
                    else
                    {

                        HttpContext.Session.SetString("IsUserGoingValidation", "yes");
                        HttpContext.Session.SetString("RegisteringMail", new_user.UserMail);

                        new_user.UserPassword = Sha256Hash.ComputeSha256Hash(new_user.UserPassword);


                        return RedirectToAction("EmailValidation", "User", new_user);
                    }
                }
                
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult EmailValidation(TableUser new_user)
        {
            if (HttpContext.Session.GetString("IsUserGoingValidation") == "yes")
            {
                try
                {

                    Random random = new Random();
                    var random_code = random.Next(1000, 9999);
                    HttpContext.Session.SetString("MailRandomCode", random_code.ToString());
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("tanitim@forumspeakup.com", "VoiceForm");
                        var receiverEmail = new MailAddress(new_user.UserMail, "Receiver");
                        var password = "Aynur123456789*";
                        var sub = "onay kodu";
                        var body = random_code;
                        var smtp = new SmtpClient
                        {
                            Host = "mail.forumspeakup.com",
                            Port = 587,
                            EnableSsl = false,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = sub,
                            Body = body.ToString()
                        })
                        {
                            smtp.Send(mess);
                        }
                        return View();
                    }
                }
                catch (Exception)
                {
                    HttpContext.Session.SetString("MailRandomCode", "mail_not_sent");
                    return RedirectToAction("Register");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Register");
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmailValidation(string enteredcode, TableUser sended_user)
        {
            var old_code = HttpContext.Session.GetString("MailRandomCode");

            if (enteredcode == null)
            {
                ViewBag.mailValidationText = "Kod girilecek kısmı boş girdiniz.";
                return View();
            }
            else if (!enteredcode.All(char.IsDigit))
            {
                ViewBag.mailValidationText = "Kod sadece sayılardan oluşmalıdır";
                return View();
            }
            else
            {
                if (enteredcode == old_code || enteredcode == "mail_not_sent")
                {      
                    user_manager.Add(sended_user);
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.mailValidationText = "Yanlış kod girdiniz.";
                    return View();
                }
            }


        }

        [HttpGet]

        public IActionResult Login()
        {
            SessionOperations.KillAllSessions(this);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(TableUser my_user)
        {
            if (my_user.UserName != null && my_user.UserPassword != null)
            {
                my_user.UserPassword = Sha256Hash.ComputeSha256Hash(my_user.UserPassword);
                var existing_user = user_manager.UserLoginBL(my_user);

                var is_user_admin = user_manager.IsUserAdminBL(my_user);

                if (is_user_admin != null)
                {
                    HttpContext.Session.SetString("IsAdminLogged", "yes");
                    HttpContext.Session.SetInt32("LoggedID", is_user_admin.UserId);

                    return RedirectToAction("AdminMainPage", "Admin");
                }
                else
                {
                    if (existing_user != null && existing_user != my_user)
                    {
                        HttpContext.Session.SetString("IsUserLogged", "yes");
                        HttpContext.Session.SetInt32("LoggedID", existing_user.UserId);
                        return RedirectToAction("index", "Main");
                    }
                    else
                    {
                        @ViewBag.validateError = "Kullanıcı adı veya şifre yanlış.";
                        return View();
                    }
                }
            }
            else
            {
                @ViewBag.validateError = "Kullanıcı adı veya şifre boş bırakılamaz.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ForgotPasswordSendMailMiddle()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPasswordSendMailMiddle(string myMail)
        {

            HttpContext.Session.SetString("IsUserGoingForgotPassword", "yes");
            HttpContext.Session.SetString("ForgotMail", myMail);
            return RedirectToAction("ForgotPasswordSendMail", "User");
        }

        [HttpGet]
        public IActionResult ForgotPasswordSendMail()
        {
            if (HttpContext.Session.GetString("IsUserGoingForgotPassword") == "yes")
            {
                try
                {
                    var sended_mail = HttpContext.Session.GetString("ForgotMail");
                    Random random = new Random();
                    var random_code = random.Next(1000, 9999);
                    HttpContext.Session.SetString("ForgotMailRandomCode", random_code.ToString());
                    ViewBag.sendedMail = sended_mail;
                    var senderEmail = new MailAddress("tanitim@forumspeakup.com", "VoiceForm");
                    var receiverEmail = new MailAddress(sended_mail, "Receiver");
                    var password = "Aynur123456789*";
                    var sub = "şifre değiştirme kodu";
                    var body = random_code;
                    var smtp = new SmtpClient
                    {
                        Host = "mail.forumspeakup.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body.ToString()
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();

                }
                catch (Exception)
                {
                    HttpContext.Session.SetString("ForgotMailRandomCode", "mail_not_sent");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPasswordSendMail(string enteredcode, string enteredmail)
        {
            if (HttpContext.Session.GetString("IsUserGoingForgotPassword") == "yes")
            {
                var old_code = HttpContext.Session.GetString("ForgotMailRandomCode");

                if (enteredcode == null)
                {
                    ViewBag.mailValidationText = "Kod girilecek kısmı boş girdiniz.";
                    return View();
                }
                else if (!enteredcode.All(char.IsDigit))
                {
                    ViewBag.mailValidationText = "Kod sadece sayılardan oluşmalıdır";
                    return View();
                }
                else
                {
                    if (enteredcode == old_code || enteredcode == "mail_not_sent")
                    {
                        HttpContext.Session.SetString("IsUserGoingChangePassword", "yes");
                        return RedirectToAction("ChangePassword", "User");
                    }
                    else
                    {
                        ViewBag.mailValidationText = "Yanlış kod girdiniz.";
                        return View();
                    }

                }
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("IsUserGoingChangePassword") == "yes")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string enteredpass1, string enteredpass2)
        {
            if (HttpContext.Session.GetString("IsUserGoingChangePassword") == "yes")
            {
                if (enteredpass1 == enteredpass2)
                {
                    var sended_mail = HttpContext.Session.GetString("ForgotMail");
                    var hashed_pass = Sha256Hash.ComputeSha256Hash(enteredpass1);
                    user_manager.ChangePasswordBL(sended_mail, hashed_pass);
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.passValidationText = "Şifreler eşleşmiyor.";
                    return View();
                }

            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            SessionOperations.KillAllSessions(this);
            return RedirectToAction("index", "Main");
        }

        [HttpGet]
        public IActionResult MyProfile()
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                ExtendedProfile new_profile = new ExtendedProfile();
                new_profile.User = user_manager.GetMyProfile(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                new_profile.shared_list = user_manager.GetUsersLastShared(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                new_profile.liked_list = user_manager.GetUsersLastLiked(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                return View(new_profile);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }

        }

        [HttpGet]
        public IActionResult GetProfile(int user_id)
        {
            ExtendedProfile new_profile = new ExtendedProfile();
            new_profile.User = user_manager.GetMyProfile(user_id);
            new_profile.shared_list = user_manager.GetUsersLastShared(user_id);
            new_profile.liked_list = user_manager.GetUsersLastLiked(user_id);
            return View(new_profile);
        }

        [HttpGet]
        public IActionResult GetAllSharings(int user_id,int page = 1, int pageSize = 10)
        {
            ProfileAllSharingsMode model = new ProfileAllSharingsMode();
            model.Userid = user_id;
            model.Topics = user_manager.GetUsersAllShared(user_id);
            model.TotalPages = model.Topics.Count() % 10 == 0 ? model.TotalPages = model.Topics.Count() / 10
    : (model.Topics.Count() / 10) + 1;
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            model.Topics = model.Topics.Skip(skip).Take(take).ToList();
            model.CurrentPage = page;
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePP()
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }              
        }

        [HttpPost("FileUpload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePP(IFormFile file)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                var username = user_manager.GetUsernameByID(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string ImageName = fileName = username + DateTime.Now.ToString("yymmssfff") + fileName + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                user_manager.ChangePP(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")), ImageName);
                return RedirectToAction("MyProfile", "User");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }         
        }

        [HttpGet]
        public IActionResult ChangeBio()
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                var user_bio = user_manager.GetBioByID(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                TextAreaModel my_model = new TextAreaModel();
                my_model.TextAreaValue = user_bio;
                return View(my_model);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeBio(TextAreaModel mymodel)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                user_manager.ChangeBio(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")), mymodel.TextAreaValue);
                return RedirectToAction("MyProfile", "User");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }     
        }

        [HttpGet]
        public IActionResult DeleteMyAccount()
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMyAccount(string mytext)
        {
            if (HttpContext.Session.GetString("IsUserLogged") == "yes")
            {
                user_manager.DeactivateUser(Convert.ToInt32(HttpContext.Session.GetInt32("LoggedID")));
                return RedirectToAction("Logout");
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult SecurityPolicy()
        {
            return View();
        }

    }


}

