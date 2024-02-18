Welcome to IdentityServer8 (latest)
=============================================

.. image:: images/logo.png
   :align: center

IdentityServer8 is an OpenID Connect and OAuth 2.0 framework for ASP.NET DotNet 8.

Browse the latest `IdentityServer8 source code onGitHub <https://github.com/alexhiggins732/IdentityServer8>`_ or download the `latest IdentyServer8 packages <https://www.nuget.org/packages/HigginsSoft.IdentityServer8//>`_ on NuGet.

.. warning:: 
   This is a revival of the archived IdentityServer4 project which started a new `company <https://duendesoftware.com/>`_ as of Oct, 1st 2020. 
   The new Duende IdentityServer is not longer free open source, but now has various commercial licenses and paid upgrade package.
   IdentityServer8 and dependenices have been upgraded to DotNet 8 and will be maintained by HigginsSoft, Alexander Higgins and the community as an Open Source project. 

.. note:: This docs cover the latest version on main branch. This might not be released yet. Use the version picker in the lower left corner to select docs for a specific version.

It enables the following features in your applications:


| **Authentication as a Service** 
| Centralized login logic and workflow for all of your applications (web, native, mobile, services). IdentityServer is an officially `certified <https://openid.net/certification/>`_ implementation of OpenID Connect.

| **Single Sign-on / Sign-out** 
| Single sign-on (and out) over multiple application types.

| **Access Control for APIs** 
| Issue access tokens for APIs for various types of clients, e.g. server to server, web applications, SPAs and native/mobile apps.

| **Federation Gateway**
| Support for external identity providers like Azure Active Directory, Google, Facebook etc. This shields your applications from the details of how to connect to these external providers.

| **Focus on Customization**
| The most important part - many aspects of IdentityServer can be customized to fit **your** needs. Since IdentityServer is a framework and not a boxed product or a SaaS, you can write code to adapt the system the way it makes sense for your scenarios.

| **Mature Open Source**
| IdentityServer uses the permissive `Apache 2 <https://www.apache.org/licenses/LICENSE-2.0>`_ license that allows building commercial products on top of it. It is also part of the `.NET Foundation <https://dotnetfoundation.org/>`_ which provides governance and legal backing.

| **Free and Commercial Support**
| If you need help building or running your identity platform, :ref:`let us know <refSupport>`. There are several ways we can help you out.

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Introduction

   intro/big_picture
   intro/architecture
   intro/terminology
   intro/specs
   intro/packaging
   intro/support
   intro/test
   intro/contributing

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Quickstarts

   quickstarts/0_overview
   quickstarts/1_client_credentials
   quickstarts/2_interactive_aspnetcore
   quickstarts/3_aspnetcore_and_apis
   quickstarts/4_javascript_client
   quickstarts/5_entityframework
   quickstarts/6_aspnet_identity
   
.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Configuration

   configuration/startup
   configuration/resources
   configuration/clients
   configuration/mvc
   configuration/apis

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Topics

   topics/startup
   topics/resources
   topics/clients
   topics/signin
   topics/signin_external_providers
   topics/windows
   topics/signout
   topics/signout_external_providers
   topics/signout_federated
   topics/federation_gateway
   topics/consent
   topics/apis
   topics/deployment
   topics/logging
   topics/events
   topics/crypto
   topics/grant_types
   topics/client_authentication
   topics/extension_grants
   topics/resource_owner
   topics/refresh_tokens
   topics/reference_tokens
   topics/persisted_grants
   topics/pop
   topics/mtls
   topics/request_object
   topics/custom_token_request_validation
   topics/cors
   topics/discovery
   topics/add_apis
   topics/add_protocols
   topics/tools

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Endpoints

   endpoints/discovery
   endpoints/authorize
   endpoints/token
   endpoints/userinfo
   endpoints/device_authorization
   endpoints/introspection
   endpoints/revocation
   endpoints/endsession

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Reference

   reference/options
   reference/identity_resource
   reference/api_scope
   reference/api_resource
   reference/client
   reference/grant_validation_result
   reference/profileservice
   reference/interactionservice
   reference/deviceflow_interactionservice
   reference/ef
   reference/aspnet_identity

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Misc

   misc/training
   misc/blogs
   misc/videos
