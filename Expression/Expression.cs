///Copyright(c) 2015,HIT All rights reserved.
///Summary:Expression
///Author:Irlovan
///Date:2015-11-13
///Description:http://csharpeval.codeplex.com/
///Modification:

using ExpressionEvaluator;
using Irlovan.Database;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Irlovan.Expression
{
    public class Expression
    {

        #region Structure

        /// <summary>
        /// Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="source"></param>
        public Expression(string expression, Catalog source = null) {
            _source = source;
            OriginExpression = expression;
            _regularExpression = new Regex(_regularString);
            _mc = _regularExpression.Matches(OriginExpression);
            GetDataList(_mc);
        }

        #endregion Structure

        #region Field

        //Filter content between {}
        private string _regularString = @"(?<=[{])[^{}]*(?=[}])";
        private Regex _regularExpression;
        private MatchCollection _mc;
        private Catalog _source;

        #endregion Field

        #region Property

        /// <summary>
        /// the origion expression not be compiled
        /// </summary>
        public string OriginExpression { get; private set; }

        /// <summary>
        /// CompiledExpression
        /// </summary>
        public string CompiledExpression { get; private set; }

        /// <summary>
        /// DataList
        /// </summary>
        public Dictionary<string, IIndustryData> DataList { get; private set; }


        #endregion Property

        #region Function

        /// <summary>
        /// get the value of the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Eval(object[] value = null) {
            CompiledExpression = (value == null) ? GetExpression() : GetExpression(value);
            var result = new CompiledExpression(CompiledExpression);
            result.Parse();
            result.Compile();
            return result.Eval();
        }

        /// <summary>
        /// GetExpression
        /// </summary>
        /// <returns></returns>
        private string GetExpression() {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (Match m in _mc) {
                sb.Append(OriginExpression.Substring(index, m.Index - index - 1));
                sb.Append(DataList[m.Value].Value.ToString());
                index = (short)(m.Index + m.Length + 1);
            }
            sb.Append(OriginExpression.Substring(index, OriginExpression.Length - index));
            return sb.ToString();
        }

        /// <summary>
        /// GetExpression
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetExpression(object[] value) {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (Match m in _mc) {
                sb.Append(OriginExpression.Substring(index, m.Index - index - 1));
                sb.Append(value[index].ToString());
                index = (short)(m.Index + m.Length + 1);
            }
            sb.Append(OriginExpression.Substring(index, OriginExpression.Length - index));
            return sb.ToString();
        }

        /// <summary>
        /// GetDataList
        /// </summary>
        /// <param name="mc"></param>
        private void GetDataList(MatchCollection mc) {
            if ((mc.Count == 0) || (_source == null)) { return; }
            DataList = new Dictionary<string, IIndustryData>();
            foreach (Match m in mc) {
                IIndustryData data = _source.AcquireIndustryData(m.Value);
                //if (data==null) {continue;}
                DataList.Add(m.Value, data);
            }
        }

        #endregion Function

    }
}
