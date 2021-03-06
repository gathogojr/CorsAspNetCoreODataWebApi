The OData Web API solution in this repo mirrors this [other](https://github.com/gathogojr/CorsAspNetCoreWebApi) Web API solution in a different repo. The key difference is the added support for OData in the OData Web API solution.

However, finegrained Cors using `EnableCors` attribute does not seem to work in the OData Web Api solution with the following configuration:

In [_CorsAspNetCoreWebApi/Startup.cs_](https://github.com/gathogojr/CorsAspNetCoreWebApi/blob/master/CorsAspNetCoreWebApi/Startup.cs#L45) - `app.UseCors()`

In [_CorsAspNetCoreWebApi/Controllers/MoviesController.cs_](https://github.com/gathogojr/CorsAspNetCoreWebApi/blob/master/CorsAspNetCoreWebApi/Controllers/MoviesController.cs#L23) `Get` controller action method - `EnableCors(Constants.AllowSpecificOriginsCorsPolicy)`