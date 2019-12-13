using AutoFixture;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dashboard.Api.Tests.Customizations
{
    public class BindingInfoCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        }
    }
}