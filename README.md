[![NuGet stable version](https://badgen.net/nuget/v/AjaxService.Gen)](https://badgen.net/nuget/v/AjaxService.Gen)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
# AjaxService.Gen

  
#### [AjaxService](https://github.com/MrAliSalehi/AjaxService.Gen) is a source-generator, it can generate *[ajax](https://www.w3schools.com/xml/ajax_intro.asp)* calls in *typescript* **based** on your c\# *api endpoints*.



# Table of contents

- [Table of contents](#table-of-contents)
- [Features](#features)
- [Demo](#demo)
    - [**models**](#models)
    - [**Endpoints**](#endpoints)
- [Install](#install)
- [How to Use](#how-to-use)
- [Recommandations](#recommandations)
    - [**only one parameter**](#i-highly-suggest-to-use-only-one-parameter-for-your-endpoint-and-put-everything-inside-it-for-example-instead-of-this)
    - [**DO NOT use circular objects**](#do-not-use-circular-parent-child-objects-example)
    - [**Use Attributes**](#use-attributes-to-specify-type-of-an-parameter-supported-types-are)
    - [**`[FromBody]` and Complex Object**](#only-one-frombody-only-one-complex-object)
    - [**Jagged/Multidimensional arrays**](#jaggedmultidimensional-arrays-are-ignored)




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


# Recommandations

### **I highly suggest to use only one parameter for your endpoint and put everything inside it, for example instead of this:**

```csharp
void UpdateUser(string name,int age,datetime birthDate){
    //update
}
```
wrap your input in one single object:
```csharp
class UpdateUserDto{
    public string name { get; set; }
    public int age { get; set; }
    public datetime birthDate { get; set; }
}
```
and use that in your endpoint:

```csharp
void UpdateUser(UpdateUserDto user){
    //update user
}
```


### **DO NOT use circular parent child objects, example:**
```csharp
class Parent{
    public string name { get; set; }
    public Child MyChild { get; set; }
}
class Child { 
    public Parent MyParent { get; set; }
}
```
it would presumably blow your pc with infinte object creations(perhaps i handled it but i just cant remember!)


### **Use Attributes to specify type of an parameter, supported types are:**
  - `[FromBody]`
  - `[FromHeader]`
  - `[FromQuery]`

:warning: if you dont specify anything with these Attributes, AjaxService by default uses `[FromBody]` for **complex objects**(classes) and `[FromQuery]` for **simple types**(string,int,bool...etc).


### **Only One `[FromBody]`, only one Complex Object**

since AjaxService does not support `[FromForm]`, you can only have one complex object as an argument, otherwise it wont be able to send the data. for example this does not work:

```csharp
void UpdateUser(User user, UserAge age){ 
    // will throw exception on compile time
}
```
also if you asign a simple type to `[FromBody]` and your endpoint has a complex type; that will also throw,e.x:
```csharp
void UpdateUser([FromBody]int age,User user){

}
```
because, **only one** argument can be passed as `[FromBody]` and there is no any other option for complex objects.


### **Jagged/Multidimensional arrays are ignored.** 

I mean, not completely, if you have the followings:

`int[][]` or `List<int[]>` or `List<int>[]` ..etc

they all will be same as `int[]`.
because it cant be fit in a request **IF their type is not [FromBody]**.

so in `[FromHeader]` and `[FromQuery]` they are same as a simple array, however you can always [put them in an object](https://github.com/MrAliSalehi/AjaxService#small_red_triangle_down-i-highly-suggest-to-use-only-one-parameter-for-your-endpoint-and-put-everything-inside-it-for-example-instead-of-this) and thay way it will work.

