---
name: simplemq-maintainer
description: "Use when: modifying, extending, refactoring, reviewing, or debugging the SimpleMq library in this monorepo (consumer registration, MQ bootstrap, message dispatching, channels, connection lifecycle, publisher behavior, and broker config validation)."
---

# SimpleMq Maintainer

## Goal

Apply safe, low-regression changes in the SimpleMq library with focus on:

- Runtime reliability
- Resource lifecycle and disposal
- Performance in hot paths
- Clear separation of responsibilities

## Project-Specific Architecture

Core flow (current baseline):

1. AddSimpleMQ extension configures options, registers services, and registers consumer metadata.
2. ConsumerRegistrationBuilder scans consumer types and builds compiled invokers.
3. MQBootstrapService sets up broker topology, creates one channel per queue, and subscribes consumers.
4. ConsumerMessageDispatcher handles per-message routing-key checks, payload parsing, scope creation, invocation, and ack or nack.
5. ConnectionService owns RabbitMQ connection and channel creation lifecycle.

Relevant files:

- libs/SimpleMQ/Extensions/AddSimpleMQ.cs
- libs/SimpleMQ/Services/ConsumerRegistrationBuilder.cs
- libs/SimpleMQ/Services/ConsumerRegistration.cs
- libs/SimpleMQ/Services/MQBootstrapService.cs
- libs/SimpleMQ/Services/ConsumerMessageDispatcher.cs
- libs/SimpleMQ/Services/ConnectionService.cs
- libs/SimpleMQ/Services/BasePublisherService.cs
- libs/SimpleMQ/Options/MessageBrokerConnectionOptions.cs
- libs/SimpleMQ/Options/PublishMessageOptions.cs

## Non-Negotiable Rules For This Repo

1. Channel strategy

- Keep one channel per queue in bootstrap runtime.
- Do not regress to one channel per handler unless explicitly requested.

2. Invocation strategy

- Keep compiled delegates for consumer invocation.
- Avoid reflection invocation in per-message hot paths.

3. DI and scope

- Resolve consumer handler instances inside a per-message async scope.
- Preserve correct scoped dependency lifetimes.

4. Ack and nack semantics

- When autoAck is false, use explicit ack on success and nack on failure.
- When autoAck is true, never attempt nack.

5. Shutdown behavior

- Track consumer tags.
- Cancel subscriptions before closing channels.
- Unsubscribe event handlers and dispose channels defensively.

6. Connection lifecycle

- Keep async-first connection handling.
- Keep disposal guards and avoid use-after-dispose behavior.

7. Options safety

- Keep startup validation for MessageBrokerConnectionOptions.
- Keep sane defaults for local development.

## Edit Strategy

When asked to change behavior:

1. Locate the responsibility boundary first:

- Registration and discovery: AddSimpleMQ or ConsumerRegistrationBuilder
- Runtime orchestration: MQBootstrapService
- Per-message processing: ConsumerMessageDispatcher
- Broker connection lifecycle: ConnectionService
- Publish metadata and serialization behavior: BasePublisherService

2. Prefer targeted edits over broad rewrites.

3. If introducing a new concern, prefer extraction into a dedicated service instead of growing existing orchestrators.

4. Preserve public API shape when possible. If API changes are required, keep compatibility wrappers where feasible.

## Performance Guidelines

- Avoid extra allocations in message-processing loops when possible.
- Avoid repeated reflection work at runtime.
- Avoid creating channels per message.
- Keep logging meaningful but do not excessively log large payload content.

## Reliability Checklist Before Finalizing

- Build/error check passes for libs/SimpleMQ.
- No new nullable warnings from touched files.
- AutoAck false path: success ack and failure nack remain correct.
- AutoAck true path: no nack call path introduced.
- Subscriptions can be cancelled and channels closed during stop.
- Connection service still behaves safely under concurrent open and dispose scenarios.

## Suggested Verification Commands

- dotnet build libs/SimpleMQ/SimpleMQ.csproj
- dotnet build monorepo-dotnet.sln

## Review Mindset For SimpleMq

When asked for review, prioritize findings in this order:

1. Message loss or duplicate risk
2. Ack and nack correctness
3. Resource leak or shutdown deadlock risk
4. Concurrency races in connection or channel lifecycle
5. Hot-path performance regressions
6. API compatibility and developer ergonomics
