[![NuGet stable version](https://badgen.net/nuget/v/AjaxService.Gen)](https://badgen.net/nuget/v/AjaxService.Gen)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
# AjaxService.Gen

  
#### [AjaxService](https://github.com/MrAliSalehi/AjaxService.Gen) is a source-generator, it can generate *[ajax](https://www.w3schools.com/xml/ajax_intro.asp)* calls in *typescript* **based** on your c\# *api endpoints*.



## Table of content



# Features

- **detects parameters and return types.**
- **create models and classes based on associated types.**
- **support [FromBody],[FromHeader],[FromQuery]** (in other words, it can handle body,header and query types).
- **validates parameter types and creates valid ajax calls.**
- **uses best practices for generated typescript code.**
- **uses built-in fetch api with asynchronous functionality.**


# Demo

### **models:**

**From:**

<img src="https://github.com/MrAliSalehi/AjaxService/blob/master/images/csharpClass.png?raw=true" width="600" height="400"/>

**To:**

<img src="https://github.com/MrAliSalehi/AjaxService/blob/master/images/tsClass.png?raw=true" width="750" height="680"/>

### **Endpoints:**

**From:**

<img src="https://github.com/MrAliSalehi/AjaxService/blob/master/images/csharpEndpoint.png?raw=true" width="500" height="320"/>

**To:**

<img src="https://github.com/MrAliSalehi/AjaxService/blob/master/images/tsEndpoint.png?raw=true" width="650" height="480"/>


# Install


dotnet cli: `dotnet add package AjaxService.Gen --version 1.0.2`

nuget: `NuGet\Install-Package AjaxService.Gen -Version 1.0.2`

reference: `<PackageReference Include="AjaxService.Gen" Version="1.0.2" />`

###


# How to Use

after [installing]() the package. you have access to `[Ajax]` attribute.

add it to your endpoint and pass dawn the url:

```csharp
[Ajax("http://mysite.com/url/to/endpoint")]
public User GetUser(int id)
{
    return new User();
}
```

then **build** your project.

it will create a `tsFiles` directory in your project and 2 files:

- `AjaxService.ts`
- `Models.ts`

Now you can use your endpoint in client code: 

import AjaxService:
```typescript
import {AjaxService} from "./AjaxService";
```
create an instance of your controller:
```typescript
let controller = new AjaxService.HomeController();
```
in my case, my controller was `HomeController`,make sure to replace it with yours.

and at the end, call the endpoint:

```typescript
let result = await controller.Endpoint_1Async();
```
to use the result you **have** to `await` the call and also check the status of call:

```typescript
if (result.IsRight()) {
    console.log(result.Value);
    return;
}
console.log(result.Error);

```
