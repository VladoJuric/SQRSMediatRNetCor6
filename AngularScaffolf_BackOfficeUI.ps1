param(
)

function Activate() {
    swagger tofile --output FrontEnds\BackOfficeUI\OpenApi\openapi_spec.json src\BackOfficeUI\bin\Debug\net6.0\BackOfficeUI.dll v1
    Remove-Item FrontEnds\BackOfficeUI\ClientApp\src\app\shared\generated -Recurse
    openapi-generator-cli generate -i FrontEnds\BackOfficeUI\OpenApi\openapi_spec_BackOfficeUI.json -g typescript-angular -o FrontEnds\BackOfficeUI\ClientApp\src\app\shared\generated -c FrontEnds\BackOfficeUI\OpenAPI\openapi_angular_config.json --type-mappings=DateTime=Date
}

Activate