using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice_Form.Models;

namespace BusinessLayer.Concrete
{
    public class TopicBL : ManagerRepository<TableTopic, TopicDAL>
    {
        TopicDAL topic_dal = new TopicDAL();

        public List<TableTopic> GetLatest()
        {
            return topic_dal.GetLatest();
        }

        public List<TableTopic> GetTrends()
        {
            return topic_dal.GetTrends();
        }

        public List<TableTopic> GetTrendsByCategory(int category_id)
        {
            return topic_dal.GetTrendsByCategory(category_id);
        }

        public TopicAndCommentsModel getTopicAndComments(int topic_id)
        {
           return topic_dal.getTopicAndComments(topic_id);
        }

        public TopicAndCommentsModel getTopicAndCommentsOrderDescendingDate(int topic_id)
        {
            return topic_dal.getTopicAndCommentsOrderDescendingDate(topic_id);
        }

        public TopicAndCommentsModel getTopicAndCommentsOrderAscendingDate(int topic_id)
        {
            return topic_dal.getTopicAndCommentsOrderAscendingDate(topic_id);
        }

        public TopicAndCommentsModel getTopicAndCommentsOrderMostLiked(int topic_id)
        {
            return topic_dal.getTopicAndCommentsOrderMostLiked(topic_id);
        }

        public string LikeTopic(int topic_id, int user_id)
        {
            return topic_dal.LikeTopic(topic_id, user_id);
        }

        public int? GetTopicLikeNumber(int topic_id)
        {
           return topic_dal.GetTopicLikeNumber(topic_id);
        }

        public List<TableTopic>? SearchTopics(string search_param)
        {
            return topic_dal.SearchTopics(search_param);
        }

        public int? GetPosterIDByTopicID(int topic_id)
        {
            return topic_dal.GetPosterIDByTopicID(topic_id);
        }
        public bool CheckIfUserCanPostToday(int? user_id)
        {
           return topic_dal.CheckIfUserCanPostToday(user_id);
        }

        public List<TableTopic> GetCategoriesPopularTopics(int sub_id)
        {
           return topic_dal.GetCategoriesPopularTopics(sub_id);
        }

        public List<TableTopic> GetLatestByCategory(int id)
        {
            return topic_dal.GetLatestByCategory(id);
        }
    }
}
