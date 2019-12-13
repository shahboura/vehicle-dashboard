using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Dashboard.Api.Tests.Customizations;

namespace Dashboard.Api.Tests
{
    public class DefaultAutoDataAttribute : AutoDataAttribute
    {
        public DefaultAutoDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoNSubstituteCustomization{ConfigureMembers = true})
                .Customize(new BindingInfoCustomization()))
        {
        }
    }
}