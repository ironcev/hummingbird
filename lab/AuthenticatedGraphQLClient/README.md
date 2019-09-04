# Authenticated GraphQL Client

## Description

When accessing [GitHub GraphQL API](https://developer.github.com/v4/) from [Insomnia](https://insomnia.rest) providing my GitHub username and password as a basic authentication was enough to authenticate the call. Here is the HTTP header that Insomnia was sending:

    > POST /graphql HTTP/1.1
    > Host: api.github.com
    > Authorization: Basic <my username:password Base64 encoded>
    > User-Agent: insomnia/6.6.2
    > Content-Type: application/json
    > Accept: */*
    > Content-Length: 60

Whe trying to access the API using [GraphQL.Client](https://github.com/graphql-dotnet/graphql-client) I couldn't get the call authenticated.

The purpose of this experiment is to see how to consume GitHub GraphQL API from C# code using GraphQL.Client or any other approach.

## Running the Experiment

The experiment exists of three projects. Each of them is self-explainable and can be run separately.

## Results

The best I got while trying to connect via GraphQL.Client was a response with Data and Error properties set to null :-( Using access token didn't work no matter how I named the authorization header (Bearer or Token (as [described in the GitHub documentation](https://developer.github.com/v3/#authentication))).

The best I got while trying to connect via vanilla [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8) was "401 Unauthorized".

In order to get *anything* with GraphQL.Client or HttpClient these HTTP headers **must** be set:

- Accept
- UserAgent

By chance while experimenting I stumbled upon [Octokit.GraphQL](https://github.com/octokit/octokit.graphql.net). (Awesome project by the way!) Octokit.GraphQL worked like a charm with the access token. So we will use it.

Octokit.GraphQL uses of course HttpClient under the hood. It would be interesting to see what it exactly sends to figure out why GraphQL.Client and HttpClient didn't work. Not so important at the moment.