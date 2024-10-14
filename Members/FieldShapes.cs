using Members.Base;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members
{
    internal class FieldShapes : IShapeTableProvider
    {
        public ValueTask DiscoverAsync(ShapeTableBuilder builder)
        {
            //builder.Describe("TextField_Edit").OnDisplaying(ctx =>
            //{
            //var def=DriverService.GetFieldDef(ctx as BuildEditorContext, false);
            //    var contentItem = ctx.Shape.TagName;
            //});

            return ValueTask.CompletedTask;
        }
    }
}
