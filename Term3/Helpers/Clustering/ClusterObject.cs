using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Helpers.Clustering
{
    public class ClusterObject
    {
        private string objectName;
        private double[] attributeArray;
        private string title;
        private int spentTime;
        private int estimateTime;

        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public double[] AttributeArray
        {
            get { return attributeArray; }
            set { attributeArray = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public int SpentTime
        {
            get { return spentTime; }
            set { spentTime = value; }
        }
        public int EstimateTime
        {
            get { return estimateTime; }
            set { estimateTime = value; }
        }

        public ClusterObject() { }

        public ClusterObject(string objectName, double[] attributeArray, string title, int spentTime, int estimateTime)
        {
            this.objectName = objectName;
            this.attributeArray = attributeArray;
            this.title = title;
            this.spentTime = spentTime;
            this.estimateTime = estimateTime;
        }
    }
}