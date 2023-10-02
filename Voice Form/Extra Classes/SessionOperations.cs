using Microsoft.AspNetCore.Mvc;

namespace Voice_Form.Extra_Classes
{
    public class SessionOperations
    {
        public static void KillAllSessions(Controller my_controller)
        {
            my_controller.HttpContext.Session.Remove("IsUserGoingValidation");
            my_controller.HttpContext.Session.Remove("RegisteringMail");
            my_controller.HttpContext.Session.Remove("MailRandomCode");
            my_controller.HttpContext.Session.Remove("IsAdminLogged");
            my_controller.HttpContext.Session.Remove("LoggedID");
            my_controller.HttpContext.Session.Remove("IsUserLogged");
            my_controller.HttpContext.Session.Remove("ForgotMail");
            my_controller.HttpContext.Session.Remove("ForgotMailRandomCodew");
        }
    }
}
