-- delete from PersistedGrants;
-- delete from IdentityResources;
-- delete from IdentityClaims;
--     delete from ClientScopes


-- delete from AspNetUserClaims
-- where Id=18


-- insert into AspNetRoleClaims(RoleId,claimType,ClaimValue)
-- values(1, "AO Encoder","AO Dashboard")

-- UPDATE Clients 
-- set AllowAccessTokensViaBrowser=1
-- where id=3
-- update ClientRedirectUris
-- set RedirectUri="http://localhost:5002/"
-- where ClientId==3;

-- update ClientPostLogoutRedirectUris
-- set PostLogoutRedirectUri="http://localhost:5002/"
-- where ClientId==3;

-- update Clients
-- set ClientUri="http://localhost:5002/"
-- where id==3


-- update Clients
-- set FrontChannelLogoutUri="https://localhost:5003/signout-oidc",BackChannelLogoutUri="https://localhost:5003/signout-oidc"
-- where Id=2
-- update ClientPostLogoutRedirectUris
-- set PostLogoutRedirectUri="http://localhost:5000/signout-callback-oidc"
-- where ClientId=1
update AspNetRoles
set NormalizedName="AO_Encoder"
where Id=1