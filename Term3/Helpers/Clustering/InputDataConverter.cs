using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TErm.Models;

namespace TErm.Helpers.Clustering
{
    public class InputDataConverter
    {
        private static List<string> DICTIONARY = new List<string>();
        private static int COUNTWORDS = 0;

        private List<string> adjektivEnde = new List<string> { "ее","ие","ые","ое","ими","ыми",
            "ей","ий","ый","ой","ем","им","ым","ом","его","ого","ему","ому","их","ых","ую",
            "юю","ая","яя","ою","ею"};

        /// <summary>
        /// Преобразует объекты списка IssuesModel в список ClusterObject
        /// </summary>
        public List<ClusterObject> convertListToClusterObject(List<IssuesModel> issuesModel)
        {
            if (DICTIONARY.Count == 0)
            {
                DICTIONARY = dictionaryInitialize(issuesModel);
                COUNTWORDS = DICTIONARY.Count();
            }            
            List<ClusterObject> clusterObject = new List<ClusterObject>();
            foreach (IssuesModel issue in issuesModel)
            {                
                clusterObject.Add(convertToClusterObject(issue));
            }
            return clusterObject;
        }

        /// <summary>
        /// Преобразует задачу в объект кластера
        /// </summary>
        public ClusterObject convertToClusterObject(IssuesModel issues)
        {
            double[] issueArray = new double[COUNTWORDS];
            string[] issueWordsArray = String.Concat(issues.name.ToLower(), " ", issues.desc.ToLower()).Split(' ');
            for (int i = 0; i < COUNTWORDS; i++)
            {
                if (issueWordsArray.Contains(DICTIONARY[i]))
                {
                    issueArray[i] = 1;
                }
                else
                {
                    issueArray[i] = 0;
                }
            }
            return new ClusterObject(issues.issue_id.ToString(), issueArray, issues.name, issues.spent_time, issues.estimate_time);
        }

        /// <summary>
        /// Возвращает список словаря
        /// </summary>
        private List<string> dictionaryInitialize(List<IssuesModel> issuesModel)
        {
            List<string> totalWordsList = new List<string>();
            List<string> dictionary = new List<string>();
            foreach (IssuesModel issue in issuesModel)
            {
                List<string> issueWordsList = String
                    .Concat(issue.name.ToLower(), " ", issue.desc.ToLower())
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty)
                    .Replace(":", string.Empty)
                    .Replace(",", string.Empty)
                    .Replace(".", string.Empty)
                    .Split(' ')
                    .ToList();    
                issueWordsList.RemoveAll(l => l.Length < 4 && l != "бд"); //удаление предлогов
                foreach (string ende in adjektivEnde) //удаление прилагательных
                {
                    issueWordsList.RemoveAll(l => l.EndsWith(ende));
                }
                totalWordsList.AddRange(issueWordsList);
            }
            foreach (string word in totalWordsList)
            {
                if (totalWordsList.Count(l => l == word) > 2)
                {
                    dictionary.Add(word);
                }
            }
            return dictionary.Distinct().ToList();
        }
    }
}