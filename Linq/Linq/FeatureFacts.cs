using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Linq
{
    public class FeatureFacts
    {
        [Fact]
        public void NewInstanceOfFeatureIsCreatedShouldReturnFeature()
        {
            Feature feature = new Feature();
            feature.Id = 1;
            Assert.Equal(new Feature().GetType(), feature.GetType());
            Assert.Equal(1, feature.Id);
        }
    }
}
