A project for testing filtering capabilities in Semantic Kernel's PostgreSQL vector store, specifically focusing on `.Contains()` and `.Any()` filters for array-based queries over list/array columns.

## What it tests

- ✅ **Filter_By_Single_Tag**: Single tag filtering using `Contains()` -> Works as expected 
- ✅ **Filter_By_Multiple_Tags_As_Literal_String**: Multiple tag filtering with OR conditions ->  Works as expected 
- ❌ **Filter_By_Tags_Array_With_Any**: Array-based filtering using `.Any()` with dynamic tag arrays -> Fails  with error "System.NotSupportedException : Unsupported method call: Enumerable.Any"

Exception:

```
System.NotSupportedException : Unsupported method call: Enumerable.Any
   at Microsoft.SemanticKernel.Connectors.SqlFilterTranslator.TranslateMethodCall(MethodCallExpression methodCall, Boolean isSearchCondition)
   at Microsoft.SemanticKernel.Connectors.SqlFilterTranslator.Translate(Expression node, Boolean isSearchCondition)
   at Microsoft.SemanticKernel.Connectors.SqlFilterTranslator.Translate(Boolean appendWhere)
   at Microsoft.SemanticKernel.Connectors.PgVector.PostgresSqlBuilder.BuildSelectWhereCommand[TRecord](NpgsqlCommand command, String schema, String tableName, CollectionModel model, Expression`1 filter, Int32 top, FilteredRecordRetrievalOptions`1 options)
   at Microsoft.SemanticKernel.Connectors.PgVector.PostgresCollection`2.GetAsync(Expression`1 filter, Int32 top, FilteredRecordRetrievalOptions`1 options, CancellationToken cancellationToken)+MoveNext()
   at Microsoft.SemanticKernel.Connectors.PgVector.PostgresCollection`2.GetAsync(Expression`1 filter, Int32 top, FilteredRecordRetrievalOptions`1 options, CancellationToken cancellationToken)+System.Threading.Tasks.Sources.IValueTaskSource<System.Boolean>.GetResult()
   at SemanticKernel.PostgresVectorStore.FilterTests.ListColumnFilterTests.Filter_By_Tags_Array_With_Any() in E:\Code\Binit\AI\SemanticKernel.PostgresVectorStore.FilterTests\ListColumnFilterTests.cs:line 53
   at SemanticKernel.PostgresVectorStore.FilterTests.ListColumnFilterTests.Filter_By_Tags_Array_With_Any() in E:\Code\Binit\AI\SemanticKernel.PostgresVectorStore.FilterTests\ListColumnFilterTests.cs:line 53
--- End of stack trace from previous location ---
```
