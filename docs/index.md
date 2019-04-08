## Starter.Net
A Starter Kit built with production in mind.
It contains things you're most likely to use in a project such as auth, monitoring, etc

## What will this contain?
Feature list for this project as of now planned are the following without any order, they may change at any time.

- [ ] Swagger Docs (`spec.json` endpoint and docs)
- [ ] Linting
- [x] Pre configured endpoints for auth
- [ ] Some basic middlewares
- [x] Docker image generation after doing CI testing
- [ ] Logging (probably with kibana, but maybe using interface instead)
- [ ] Social Logins (facebook, linkedIn, etc)
- [ ] Single Sign In (maybe?)
- [ ] Recaptcha support
- [x] `ViewModel` to ensure communication is completely TypeSafe.
- [x] Auth with permission model
- [x] Email support with driver architecture (so you can swap out your provider)
- [ ] Easy Email Creation (although this might just be a separate project)
- [ ] Feature Toggles (this isn't just simple on/off toggles, rather toggles for group of users,
- [ ] imagine you only want to turn on some feature to some group of people, or only 1% of your customer)
- [ ] Experimentation (think A/B testing, it'll use similar concept from feature toggles, or will replace feature toggles)
- [ ] Health Reporting
- [ ] Caching
- [ ] Messaging via RabbitMQ
- [ ] Measurements (monitoring of CPU usage, disk space, http status codes, error rates, other meters etc)
