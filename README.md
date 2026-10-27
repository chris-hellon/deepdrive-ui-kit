# DeepDrive UI Kit

A dark, editorial **admin + forms component library for Blazor**, with a matching Tailwind design system.

It gives you a complete **generic CRUD flow** — searchable, paginated list pages with delete / activate-deactivate, plus create and edit pages with validation, confirmation dialogs and toasts — driven entirely by your typed [EasyApi](https://www.nuget.org/packages/BlazorUtils.EasyApi) contracts. On top of that sits a consistent set of form inputs and an "art-direction" visual theme (Sora display type, monospace instrument readouts, warm paper + cinematic dark plates, and a topographic contour texture).

It was extracted from the [DeepDrive](https://deepdrivegolf.com) golf platform and is published here as a standalone, buildable package with a runnable demo.

> **Status:** early (`0.1.0`). The API mirrors what runs in production, but namespaces and packaging may still change.

---

## What's in the box

| Area | Highlights |
|------|------------|
| **Admin framework** | `GenericAdminListPage`, `GenericAdminCreatePage`, `GenericAdminEditPage`, `AdminTable`, pagination, skeleton loaders, danger-zone delete, row action buttons |
| **Form components** | `DeepDriveInputText` / `InputNumber<T>` / `InputTextarea` / `Select` / `SearchSelect<T>` / `DatePicker` / `DateTimePicker`, `AppButton`, tooltips, FluentValidation messages |
| **Design system** | Tailwind tokens + `.dd-*` utilities (`dd-contour`, `dd-plate`, `dd-scrim`, `dd-mono`), Sora type scale, `.btn*` button system, emerald accent |
| **Toasts** | Static `ToastBroker` + `ToastContainer` |

---

## Repository layout

```
DeepDrive.UiKit.slnx
├── src/DeepDrive.UiKit/            # the Razor Class Library (the package)
│   ├── Abstractions/               # IFormModel, ISearchQuery, INamedEntity, IImageUploader, …
│   ├── Admin/                      # the generic CRUD framework + AdminTable
│   ├── Forms/                      # DeepDrive* input components
│   ├── Common/                     # CssBuilder, ConfirmDialog
│   ├── Toasts/                     # ToastBroker + ToastContainer
│   ├── Styles/input.css            # Tailwind entry (source)
│   ├── tailwind.config.js          # design tokens
│   ├── package.json                # `npm run build` → wwwroot/css/deepdrive-ui.css
│   └── wwwroot/css/deepdrive-ui.css# the built stylesheet (committed; no npm needed to consume)
└── samples/DeepDrive.UiKit.Demo/   # a runnable Blazor Server demo (in-memory "Widgets" CRUD)
```

---

## Running the demo

```bash
dotnet run --project samples/DeepDrive.UiKit.Demo
```

Then open the printed URL. The demo has three pages: a landing page, an **Admin demo** (`/widgets`) that drives the full CRUD framework against an in-memory store, and a **Components** gallery (`/components`). No database and no npm step required — the stylesheet is committed.

---

## Using it in your own app

### 1. Reference the library

```xml
<ProjectReference Include="..\DeepDrive.UiKit\DeepDrive.UiKit.csproj" />
```

### 2. Link the stylesheet and fonts

In your host page (`App.razor` / `index.html`):

```html
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&family=Poppins:wght@500;600;700;800&display=swap" />
<link rel="stylesheet" href="_content/DeepDrive.UiKit/css/deepdrive-ui.css" />
```

### 3. Register EasyApi (server side)

```csharp
builder.Services
    .AddEasyApi()
    .WithContract(typeof(Program).Assembly)  // where your IGet/IPost contracts live
    .WithServer();                            // dispatch ICall straight to your IHandle<,>

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly); // FluentValidation
```

### 4. Make your contracts implement the kit's interfaces

```csharp
public sealed class WidgetDto : IActivatableNamedEntity { /* Id, Name, IsActive, … */ }

[Route("api/widgets/search")]
public sealed class SearchWidgetsQuery : IGet<SearchWidgetsResult>, IActiveFilterableQuery { … }

public sealed class SearchWidgetsResult : ISearchResult<WidgetDto> { … }

[Route("api/widgets/delete")]
public sealed class DeleteWidgetCommand : IPost<bool>, IDeleteByIdCommand { public Guid Id { get; set; } }
```

### 5. Drop in a list page

```razor
@page "/widgets"

<GenericAdminListPage @ref="_listPage"
                      TItem="WidgetDto" TResult="SearchWidgetsResult"
                      TSearchQuery="SearchWidgetsQuery" TDeleteCommand="DeleteWidgetCommand"
                      Title="Widgets" EntityName="Widget" CreateUrl="/widgets/create"
                      SupportsDelete="true" SupportsToggleActive="true" SupportsActiveFilter="true"
                      ToggleActiveCallback="OnToggleActive">
    <TableHeaderContent>
        <AdminTableHeader Title="Name" />
        <AdminTableHeader Title="Actions" Align="TableAlign.Right" />
    </TableHeaderContent>
    <RowTemplate Context="item">
        <WidgetRow Widget="item" ListPage="_listPage" />
    </RowTemplate>
</GenericAdminListPage>
```

See `samples/DeepDrive.UiKit.Demo/Components/Pages/Widgets/` for the full list / create / edit trio and a form model.

### Rebuilding the stylesheet

Only needed if you change the Tailwind config or add components that use new utility classes:

```bash
cd src/DeepDrive.UiKit
npm install
npm run build        # → wwwroot/css/deepdrive-ui.css (+ .min.css)
```

The Tailwind `content` globs scan the library and the demo; point them at **your** project too if you consume the components so your markup's classes are emitted.

---

## Notes & limitations

- **Image uploads** are decoupled behind `IImageUploader` — the kit ships no implementation (the original product used Azure Blob Storage). Register your own to enable rich media. The Quill-based `RichTextEditorWithUpload` component from the product is **not** included in this first cut.
- **Geolocation / location autocomplete** inputs from the product are intentionally omitted (service-coupled).
- Targets **.NET 9**. Depends on `BlazorUtils.EasyApi`, `FluentValidation`, `Blazored.FluentValidation`.

---

## License

[MIT](LICENSE) © 2026 Chris Hellon
