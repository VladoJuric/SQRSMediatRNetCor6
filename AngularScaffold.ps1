param(
)

function Activate() {
    dotnet swagger tofile --output src\WebUI\OpenApi\openapi_spec.json src\WebUI\bin\Debug\net6.0\WebUI.dll v1
    <#Remove-Item src\WebUI\ClientApp\src\app\shared\generated -Recurse
    openapi-generator-cli generate -i src\WebUI\OpenApi\openapi_spec_WebUI.json -g typescript-angular -o src\WebUI\ClientApp\src\app\shared\generated -c src\WebUI\OpenAPI\openapi_angular_config.json --type-mappings=DateTime=Date#>
}

Activate