using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bonsai.Design;

namespace Bonsai.Gui
{
    internal class DataMemberSelectorEditor : MemberSelectorEditor
    {
        public DataMemberSelectorEditor()
            : base(GetDataElementType, allowMultiSelection: false)
        {
        }

        static Type GetDataElementType(Expression expression)
        {
            var parameterType = expression.Type.GetGenericArguments()[0];
            return ExpressionHelper.GetGenericTypeBindings(typeof(IList<>), parameterType).FirstOrDefault() ?? parameterType;
        }
    }
}
