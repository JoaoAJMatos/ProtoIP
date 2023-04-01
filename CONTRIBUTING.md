# Contribuiting

> Software is better when it's free. Help us maintain the project and help other students.

You can contribuite to the project by **reporting bugs**, **sugesting changes**, or by **contribuiting directly to the source tree**.

## How to contribuite to the source code

1. Create a **fork** of the **ProtoIP** repository.
2. Create a **branch** for your feature or change.
3. **Commit** your changes and **push** them to your remote repo.
4. Open a **pull-request** and wait for approval.

### Contribuiting rules

- Commit messages must be **clear** and **brief**.
- Commit messages must be preceded by the **type** of change in the commit (*check [commit types]()*).
- Referencing other commits or pull requests by their short **hash** will be appreciated when needed.
- Commits must include a detailed description of the changes (**if the changes aren't self-explanatory**).
- Pull-request messages must describe the changes and provide the **context** and **reasoning** for those same changes.
- Code must be **clear** and **well structured** following the project's naming and structuring standards.

#### Commit types

Following are the *commit tags* that should precede every commit message if it is to be accepted into the source tree:

- `refactor` - For structural changes in the code that **do not** affect behaviour.
- `p2p` - For changes to the **peer-to-peer interfaces**.
- `client-server` - For changes to the **client-server interfaces**.
- `crypto` - For changes to the **cryptography interfaces**.
- `core` - For changes in other modules.
- `docs` - For changes in the **documentation**.
- `git` - For changes regarding the **git repository**.
- `feature` - For introducing a new feature into the library.

> Commit example

```bash
git commit -m "docs: Updates ProtoStream documentation" -m "Updates the documentation for the Receive() method to reflect changes in 0f31e6c"
```

## Reporting Bugs

If you encounter any major issues that you cannot solve yourself please feel free to open a new [issue]() under the `bug` tag.

You should **describe** the issue as much as possible, as well as the **context** of when it happened. **Code snippets** to reproduce the issue are welcomed.

## Sugestions

To sugest changes, please open a new [issue]() under the `sugestion` tag.
