﻿using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.InstallationValidator.Core.Services;

namespace OSPSuite.InstallationValidator.Services
{
   public abstract class concern_for_OutputComparisonResultToCurveChartMapper : ContextSpecification<OutputComparisonResultToCurveChartMapper>
   {
      protected IDimensionFactory _dimensionFactory;
      protected IOutputResultToDataRepositoryMapper _outputResultToDataRepositoryMapper;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _outputResultToDataRepositoryMapper = A.Fake<IOutputResultToDataRepositoryMapper>();
         sut = new OutputComparisonResultToCurveChartMapper(_outputResultToDataRepositoryMapper, _dimensionFactory);

         A.CallTo(() => _outputResultToDataRepositoryMapper.MapFrom(A<OutputResult>._)).ReturnsLazily(() => new DataRepository());
      }

      public class When_the_mapper_maps_from_an_output_comparison : concern_for_OutputComparisonResultToCurveChartMapper
      {
         private OutputComparisonResult _outputComparisonResult;
         private List<CurveChart> _repositories;

         protected override void Context()
         {
            base.Context();
            _outputComparisonResult = new OutputComparisonResult("path", ValidationState.Invalid, string.Empty);
         }

         protected override void Because()
         {
            _repositories = sut.MapFrom(_outputComparisonResult).ToList();
         }

         [Observation]
         public void the_repositories_should_be_configured_correctly()
         {
            _repositories.Count(x => x.DefaultYAxisScaling == Scalings.Linear).ShouldBeEqualTo(1);
            _repositories.Count(x => x.DefaultYAxisScaling == Scalings.Log).ShouldBeEqualTo(1);

            _repositories.Count(x => x.Title == _outputComparisonResult.Path).ShouldBeEqualTo(2);
            _repositories.Count(x => x.ChartSettings.LegendPosition == LegendPositions.RightInside).ShouldBeEqualTo(2);
         }

         [Observation]
         public void two_repositories_should_be_returned()
         {
            _repositories.Count.ShouldBeEqualTo(2);  
         }
      }      
   }


}
