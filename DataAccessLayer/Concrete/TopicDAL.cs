using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Voice_Form.Models;

namespace DataAccessLayer.Concrete
{
    public class TopicDAL : EfRepositoryBase<TableTopic, VoiceFormContext>
    {
        public List<TableTopic> GetLatest()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_list = my_context.TableTopics.Where(t => t.TopicActive == true).OrderByDescending(t => t.TopicDate).Take(10).ToList();
                return my_list;
            }
        }

        //public List<TableTopic> GetTrendsOld()
        //{
        //    using (VoiceFormContext my_context = new VoiceFormContext())
        //    {
        //        var my_list = my_context.TableTopics.Where(t => t.TopicActive == true).
        //            OrderByDescending(t => (t.TopicLikes + t.TopicDislikes) + (t.TopicLikes - t.TopicDislikes)).
        //            Where(t => t.TopicDate > DateTime.Now.AddDays(-2)).Take(10).ToList();
        //        return my_list;
        //    }
        //}

        public List<TableTopic> GetTrends()
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                List<TableTopic> my_list = new List<TableTopic>();
                var topicsWithComments = my_context.TableTopics
              .Join(my_context.TableComments, t => t.TopicId, c => c.CommentTopicId, (t, c) => new { Topic = t, Comment = c })
              .Where(c => c.Comment.CommentDate > DateTime.Now.AddDays(-2))
              .GroupBy(tc => tc.Topic).ToList()
              .Select(g => new { Topic = g.Key, CommentCount = g.Count() })
              .OrderByDescending(tc => tc.CommentCount).Take(10).ToList();
                foreach (var topics in topicsWithComments)
                {
                    my_list.Add(topics.Topic);
                }
                return my_list;
            }

        }


        public List<TableTopic> GetTrendsByCategory(int category_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_list = my_context.TableTopics.Where(t => t.TopicActive == true).
                    OrderByDescending(t => (t.TopicLikes + t.TopicDislikes) + (t.TopicLikes - t.TopicDislikes)).
                    Where(t => t.TopicDate > DateTime.Now.AddDays(-2) && t.TopicCategory == category_id).ToList();
                return my_list;
            }
        }

        public TopicAndCommentsModel getTopicAndComments(int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                TopicAndCommentsModel mymodel = new TopicAndCommentsModel();
                var topic = (from t in my_context.TableTopics
                             join u in my_context.TableUsers on t.TopicSharerId equals u.UserId
                             where t.TopicId == topic_id
                             select new TopicWithUserModel
                             {
                                 Topic = t,
                                 User = u
                             }).FirstOrDefault();
                var listofcomments = (from c in my_context.TableComments
                                      join u in my_context.TableUsers on c.CommentSenderId equals u.UserId
                                      where c.CommentActive == true && c.CommentTopicId == topic_id
                                      orderby c.CommentLike descending, c.CommentDate descending
                                      select new CommentWithUserModel
                                      {
                                          Comment = c,
                                          User = u
                                      }).ToList();
                mymodel.topic = topic;
                mymodel.comments = listofcomments;
                return mymodel;
            }
        }

        public TopicAndCommentsModel getTopicAndCommentsOrderDescendingDate(int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                TopicAndCommentsModel mymodel = new TopicAndCommentsModel();
                var topic = (from t in my_context.TableTopics
                             join u in my_context.TableUsers on t.TopicSharerId equals u.UserId
                             where t.TopicId == topic_id
                             select new TopicWithUserModel
                             {
                                 Topic = t,
                                 User = u
                             }).FirstOrDefault();
                var listofcomments = (from c in my_context.TableComments
                                      join u in my_context.TableUsers on c.CommentSenderId equals u.UserId
                                      where c.CommentActive == true && c.CommentTopicId == topic_id
                                      orderby c.CommentDate descending
                                      select new CommentWithUserModel
                                      {
                                          Comment = c,
                                          User = u
                                      }).ToList();
                mymodel.topic = topic;
                mymodel.comments = listofcomments;
                return mymodel;
            }
        }

        public TopicAndCommentsModel getTopicAndCommentsOrderAscendingDate(int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                TopicAndCommentsModel mymodel = new TopicAndCommentsModel();
                var topic = (from t in my_context.TableTopics
                             join u in my_context.TableUsers on t.TopicSharerId equals u.UserId
                             where t.TopicId == topic_id
                             select new TopicWithUserModel
                             {
                                 Topic = t,
                                 User = u
                             }).FirstOrDefault();
                var listofcomments = (from c in my_context.TableComments
                                      join u in my_context.TableUsers on c.CommentSenderId equals u.UserId
                                      where c.CommentActive == true && c.CommentTopicId == topic_id
                                      orderby c.CommentDate ascending
                                      select new CommentWithUserModel
                                      {
                                          Comment = c,
                                          User = u
                                      }).ToList();
                mymodel.topic = topic;
                mymodel.comments = listofcomments;
                return mymodel;
            }
        }

        public TopicAndCommentsModel getTopicAndCommentsOrderMostLiked(int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                TopicAndCommentsModel mymodel = new TopicAndCommentsModel();
                var topic = (from t in my_context.TableTopics
                             join u in my_context.TableUsers on t.TopicSharerId equals u.UserId
                             where t.TopicId == topic_id
                             select new TopicWithUserModel
                             {
                                 Topic = t,
                                 User = u
                             }).FirstOrDefault();
                var listofcomments = (from c in my_context.TableComments
                                      join u in my_context.TableUsers on c.CommentSenderId equals u.UserId
                                      where c.CommentActive == true && c.CommentTopicId == topic_id
                                      orderby c.CommentLike descending
                                      select new CommentWithUserModel
                                      {
                                          Comment = c,
                                          User = u
                                      }).ToList();
                mymodel.topic = topic;
                mymodel.comments = listofcomments;
                return mymodel;
            }
        }

        public string LikeTopic(int topic_id, int user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_like_record = my_context.TableLikes.Where(l => l.LikeTopic == topic_id && l.LikeUser == user_id).FirstOrDefault();
                if (my_like_record != null)
                {
                    if (my_like_record.LikeActive == false)
                    {
                        var selected_topic = my_context.TableTopics.Find(topic_id);
                        selected_topic.TopicLikes = selected_topic.TopicLikes + 1;
                        my_like_record.LikeActive = true;
                        //var deleting_entry = my_context.TableLikes.Entry(my_like_record);
                        //deleting_entry.State = EntityState.Deleted;
                        my_context.SaveChanges();
                        return "user_already_liked";
                    }
                    else
                    {
                        var selected_topic = my_context.TableTopics.Find(topic_id);
                        selected_topic.TopicLikes = selected_topic.TopicLikes - 1;
                        my_like_record.LikeActive = false;
                        //var deleting_entry = my_context.TableLikes.Entry(my_like_record);
                        //deleting_entry.State = EntityState.Deleted;
                        my_context.SaveChanges();
                        return "user_already_liked";
                    }

                }
                else
                {
                    var selected_topic = my_context.TableTopics.Find(topic_id);
                    selected_topic.TopicLikes = selected_topic.TopicLikes + 1;
                    TableLike new_like = new TableLike();
                    new_like.LikeTopic = topic_id;
                    new_like.LikeUser = user_id;
                    my_context.TableLikes.Add(new_like);
                    my_context.SaveChanges();
                    return "user_liked_it";
                }
            }
        }

        public List<TableTopic>? SearchTopics(string search_param)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var exactMatches = my_context.TableTopics.Where(item => item.TopicTitle.ToLower() == search_param).ToList(); // Kesin eşleşmeleri bulma

                var possibleMatches = my_context.TableTopics.Where(item => item.TopicContent.ToLower().Contains(search_param) && !exactMatches.Contains(item)).ToList(); // Olası sonuçları bulma

                var searchResults = exactMatches.Concat(possibleMatches).ToList(); // Kesin eşleşmeleri ve olası sonuçları birleştirme

                return searchResults;
            }
        }

        public int? GetTopicLikeNumber(int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var like_num = my_context.TableTopics.Find(topic_id).TopicLikes;
                return like_num;
            }
        }

        public int? GetPosterIDByTopicID(int topic_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var poster = my_context.TableTopics.Find(topic_id).TopicSharerId;
                return poster;
            }
        }

        public bool CheckIfUserCanPostToday(int? user_id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var user = my_context.TableUsers.Find(user_id);
                var list_of_topics_today = my_context.TableTopics.
                    Where(t => t.TopicDate > DateTime.Now.AddDays(-1) && (t.TopicSharerId == user_id || t.TopicSharerIp == user.UserIpAdress)).ToList();
                if (list_of_topics_today.Count() >= 10)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public List<TableTopic> GetCategoriesPopularTopics(int sub_id)
        {
                using (VoiceFormContext my_context = new VoiceFormContext())
                {
                    List<TableTopic> my_list = new List<TableTopic>();
                    var topicsWithComments = my_context.TableTopics
                  .Join(my_context.TableComments, t => t.TopicId, c => c.CommentTopicId, (t, c) => new { Topic = t, Comment = c })
                  //.Where(c=>c.Comment.CommentDate > DateTime.Now.AddDays(-2))
                  .GroupBy(tc => tc.Topic).ToList()
                  .Select(g => new { Topic = g.Key, CommentCount = g.Count() })
                  .OrderByDescending(tc => tc.CommentCount).Where(tc=>tc.Topic.TopicCategory==sub_id).ToList();
                    foreach (var topics in topicsWithComments)
                    {
                        my_list.Add(topics.Topic);
                    }
                    return my_list;
                }
        }

        public List<TableTopic> GetLatestByCategory(int id)
        {
            using (VoiceFormContext my_context = new VoiceFormContext())
            {
                var my_list = my_context.TableTopics.Where(t => t.TopicActive == true && t.TopicSubCategory==id).OrderByDescending(t => t.TopicDate).Take(10).ToList();
                return my_list;
            }
        }
    }
}

