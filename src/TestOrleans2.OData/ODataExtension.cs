namespace TestOrleans2.OData;
public class ODataExtension
{
    public void AddAppOData(IMvcBuilder mvcBuilderControllers) {
        mvcBuilderControllers.AddOData((odataOptions) => {
            odataOptions.EnableQueryFeatures().Select().Filter().OrderBy();
            var edmModel = GetEdmModel();
            odataOptions.AddRouteComponents("odata", edmModel);
        });
    }

    private IEdmModel GetEdmModel() {
        var builder = new ODataConventionModelBuilder();
        //builder.EntitySet<ToDo>("ToDo");
        return builder.GetEdmModel();
    }

}
