using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Serialize.Linq.Nodes;

namespace E.AutoTable.Configurations
{
    public class TableOptions<TEntity>
    {
        public Type ContextType { get; set; }
        public Type EntityType { get; set; }
        public ExpressionNode<Expression<Func<TEntity, bool>>> WhereExpression { get; set; }
        public ExpressionNode OrderExpression { get; set; }
    }
}
