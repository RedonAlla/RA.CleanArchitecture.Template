# 🧭 Where to Put the Mapping Logic
In **Clean Architecture**, the mapping between **Application DTOs** and **External request/response** models (like API contracts or transport models) should be placed in the **outermost layer** —typically the **Presentation Layer** or **Infrastructure Layer**, depending on your architecture's boundaries.

## ✅ Presentation Layer (e.g., Web/API Layer)
*  ⁠This is the most common and recommended place for mapping between external models and internal DTOs.
*  ⁠Keeps your **Application Layer** clean and unaware of external concerns.
*  ⁠Allows flexibility to change external contracts without affecting core logic.

> Example:
> use controllers or dedicated mappers to convert ⁠`CreateUserRequest` ⁠→ ⁠`CreateUserDto`⁠.

## ✅ Infrastructure Layer (if dealing with external services)
* ⁠If you're integrating with external systems (e.g., REST APIs, message queues), map external payloads to Application DTOs here.
* ⁠Keeps external dependencies isolated from your core logic.

> Example:
> Mapping a third-party API response to your internal⁠ `UserDto` inside an API client class.

## 🔄 How to Structure the Mapping
* Use **mapping classes** or **AutoMapper profiles** in the **Presentation** or **Infrastructure** layer.
* ⁠Keep mappings close to where the models are used, following the principle of locality.

## 🚫 Avoid Mapping in These Layers
* ❌ **Domain Layer** Should remain pure and unaware of external models.
* ❌ **Application Layer** (mostly) Should only deal with internal DTOs and use cases

## 🧠 Summary
* **⁠External → Application DTO:** Map in **Presentation** or **Infrastructure** layer.
* **⁠Application DTO → External:** Same—keep it outside the core.