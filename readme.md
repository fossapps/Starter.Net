## Introduction
I'm starting this project so anyone can take this and kick start the project.
This will use .NET Core framework so it's truly cross-platform
(unless you make system calls to a specific platform)
Development of this project will happen from both Linux and Windows systems to make sure
that it'll will work on windows machines,
There are plans to setup testing on Linux, Windows and Mac OSes, however they're not finalized yet.

## Comparision with dotnet generator
You might be tempted to think why can't we simply do `dotnet new webapi` and we obviously can for playing around.
I want to include much more features than just returning few values from `ValuesController`

## What will this contain?
This is supposed to be batteries included but removable type of kit.
As of now I see the following to be included on this kit:
- Swagger Docs (`spec.json` endpoint and docs)
- Linting
- Pre configured endpoints for auth
- Some basic middlewares
- Docker image generation after doing CI testing
- Logging (probably with kibana, but maybe using interface instead)
- Social Logins (facebook, linkedIn, etc)
- Single Sign In (maybe?)
- Recaptcha support
- `ViewModel` to ensure communication is completely TypeSafe.
- Basic Auth with permission model
- Email support with driver architecture (so you can swap out your provider)
- Easy Email Creation (although this might just be a separate project)
- Feature Toggles (this isn't just simple on/off toggles, rather toggles for group of users,
  imagine you only want to turn on some feature to some group of people, or only 1% of your customer)
- Experimentation (think A/B testing, it'll use similar concept from feature toggles, or will replace feature toggles)
- Health Reporting
- Caching
- Messaging via RabbitMQ
- Measurements (monitoring of CPU usage, disk space, http status codes, error rates, other meters etc)

## Timeline
Well, this isn't a full time project which I can work on, so the more contribution I get the faster project will flow.

I'm thrilled about this project and want to use for myself anyway, there'll be continuous development.
Further contribution will only make this grow better and faster.
I want to see this project be production ready, where you clone it,
and start writing production ready code without setting up

So in conclusion, there's no timeline as of now, see issues to track what's being worked on and which milestone.

## Used packages

## Getting Started
Simply clone this repository, and open in Rider or Visual Studio Code and start building.

## More Documentation
Before a feature is worked on, I'll try to document on what needs to be done, after the feature is ready,
I'll try to finalize the documentation. If there's something missing, please contribute.
