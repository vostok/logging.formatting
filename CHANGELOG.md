## 1.0.12 (11-03-2022):

Format bytes array to base64 string.

## 1.0.11 (01-03-2022):

Supported properties rendering right inside Operation Context property.

## 1.0.10 (25-01-2022):

Increased max object rendered deep from 3 to 10.

## 1.0.8 (06-12-2021):

Added `net6.0` target.

## 1.0.6 (11.06.2021):

Allow dot character in property names.

## 1.0.5 (29.09.2020):

* LogMessageFormatter: no additional allocations when message template is not a template at all.

## 1.0.4 (26.09.2019):

* Fixed lowerCamelCase `WellKnownProperties`.

## 1.0.3 (26.06.2019):

* Made `ITemplateToken` interface public. `OutputTemplateBuilder` now also publicly accepts arbitrary tokens.

## 1.0.2 (29.04.2019):

* Just allowed @ character in property names.

## 1.0.1 (26.04.2019):

* Leading @ character in property names in template strings is now ignored.

## 1.0.0 (11.03.2019):

* PropertiesToken no longer renders properties mentioned elsewhere in the output template.
* Default OutputTemplate now contains all well-known properties: `TraceContext`, `SourceContext` and `OperationContext`.

## 0.1.0 (06-09-2018): 

Initial prerelease.

## 0.1.1 (18-01-2019):

Update source dependencies.
