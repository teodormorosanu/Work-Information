# Code Review & Refactoring

---

## I. Some S.O.L.I.D principles were not respected

### 1. Dependency inversion principle
1. *Which principle is not respected:* Dependency inversion principle
2. *Where in code:* `Controllers/ItemController.cs` (now `GradesController.cs`), inside the `GetAll()` and `GetById()` methods.
3. *Why it is not respected:* The controller directly used `Console.WriteLine` for logging. This violates "D" principle because the high-level controller depends on a low-level, concrete implementation (`Console`) instead of an abstraction.
4. *The fix I applied:* Injected the standard `ILogger<GradesController>` interface through the constructor.

### 2. Single responsability principle
1. *Which principle is not respected:* Single responsability principle
2. *Where in code:* `Controllers/ItemController.cs` (now `GradesController.cs`), inside the `GetAll()` method.
3. *Why it is not respected:* The controller was manually calculating business logic, specifically the total count and average value of the items. A controller's single responsability should be to receive the HTTP request, delegate processing, and return the HTTP response. Also controller has taken on the responsability of managing how logs are written, which is outside its primary scope of handling HTTP requests.
4. *The fix I applied:* Extracted the calculation logic out of the HTTP action and into a strongly-typed helper method (`GetInfo`) that returns a C# `record` (`GradeStats`). Replaced all hardcoded `Console.WriteLine` calls with `_logger.LogInformation` and `_logger.LogWarning`.

---

## II. Runtime errors

### Runtime crash
1. *Where in code:* `Program.cs`
2. *The issue:* The `ItemController` requested an `IItemReader` interface via its constructor, but no concrete implementation was registered. This would result in an Internal Server Error (`InvalidOperationException`) upon the first request.
3. *The fix I applied:* Registered the repository by adding `builder.Services.AddSingleton<IGradeReader, GradeRepository>();`. A `Singleton` lifetime was chosen explicitly to preserve the state of the `List<Grade>` across multiple HTTP requests.

---

## III. Clean code

### Generic conventions
1. *Where in code:* Across the entire project (Models, Interfaces, Repositories, Controllers).
2. *The issue:* The entities and classes were named generically (`Item`, `ItemController`, `IItemReader`). These names lacked context and did not reflect the "GradeBook".
3. *The fix I applied:* Renamed files, classes, and variables to reflect the domain:
   * `Item` -> `Grade`
   * `ItemController` -> `GradesController`
   * `IItemReader` -> `IGradeReader`
   * `ItemRepository` -> `GradeRepository`

---

## IV. Project configuration

### Default launch URL
1. *Where in code:* `Properties/launchSettings.json`
2. *The issue:* After renaming the main controller to `GradesController` (and implicitly changing its route to `api/grades`), the application's launch profiles were still pointing to the non-existent `api/item` URL.
3. *The fix I applied:* Updated the `launchUrl` property within the active profiles in `launchSettings.json` from `"api/item"` to `"api/grades"`.