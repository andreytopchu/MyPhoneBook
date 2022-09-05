using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Shared.ApiVersion
{
    public class ApiExplorerVersionConvention : IControllerModelConvention, IActionModelConvention
    {
        private readonly Microsoft.AspNetCore.Mvc.ApiVersion _defaultApiVersion;
        private string DefaultVersion => _defaultApiVersion.ToString();

        public ApiExplorerVersionConvention(Microsoft.AspNetCore.Mvc.ApiVersion defaultApiVersion)
        {
            _defaultApiVersion = defaultApiVersion ?? throw new ArgumentNullException(nameof(defaultApiVersion));
        }

        public void Apply(ControllerModel controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
        }

        public void Apply(ActionModel action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
        }
    }
}