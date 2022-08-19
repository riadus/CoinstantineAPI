using CoinstantineAPI.IntegrationTests.Configuration;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer(
CustomTestCollectionOrderer.TypeName,
CustomTestCollectionOrderer.AssembyName)]
[assembly: TestCaseOrderer(
CustomTestCaseOrderer.TypeName,
CustomTestCaseOrderer.AssembyName)]
