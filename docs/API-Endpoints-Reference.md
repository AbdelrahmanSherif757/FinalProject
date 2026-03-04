# ONEX API Complete Endpoints Reference

## ?? Overview

This document provides a complete reference for all API endpoints in the ONEX system.

**Base URL:** `http://localhost:5000` (Development)

**Authentication:** JWT Bearer Token (unless marked as Public)

---

## ?? Authentication Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/users/register` | Register new user | ?? Public |
| `POST` | `/users/login` | Login user | ?? Public |
| `POST` | `/users/logout` | Logout user | ? Required |
| `POST` | `/users/refresh` | Refresh access token | ?? Public |

### Register

```http
POST /users/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response (201):**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

### Login

```http
POST /users/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "abc123def456...",
  "expiresAt": "2024-12-27T02:00:00Z",
  "refreshTokenExpiresAt": "2025-01-03T01:00:00Z"
}
```

### Logout

```http
POST /users/logout
Authorization: Bearer {accessToken}
```

**Response:** `204 No Content`

### Refresh Token

```http
POST /users/refresh
Content-Type: application/json

{
  "refreshToken": "abc123def456..."
}
```

**Response (200):** Same as Login

---

## ?? Password Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/users/change-password` | Change password | ? Required |
| `POST` | `/users/forgot-password` | Request password reset | ?? Public |
| `POST` | `/users/reset-password` | Reset password with token | ?? Public |

### Change Password

```http
POST /users/change-password
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword456!"
}
```

### Forgot Password

```http
POST /users/forgot-password
Content-Type: application/json

{
  "email": "user@example.com"
}
```

### Reset Password

```http
POST /users/reset-password
Content-Type: application/json

{
  "email": "user@example.com",
  "token": "reset-token-from-email",
  "newPassword": "NewPassword456!"
}
```

---

## ?? Email Confirmation

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/users/confirm-email` | Confirm email address | ?? Public |
| `POST` | `/users/resend-confirmation` | Resend confirmation email | ?? Public |

### Confirm Email

```http
POST /users/confirm-email
Content-Type: application/json

{
  "userId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "token": "confirmation-token-from-email"
}
```

### Resend Confirmation

```http
POST /users/resend-confirmation
Content-Type: application/json

{
  "email": "user@example.com"
}
```

---

## ?? Two-Factor Authentication

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/users/2fa/enable` | Enable 2FA | ? Required |
| `POST` | `/users/2fa/verify` | Verify 2FA code | ? Required |
| `POST` | `/users/2fa/disable` | Disable 2FA | ? Required |

### Enable 2FA

```http
POST /users/2fa/enable
Authorization: Bearer {accessToken}
```

**Response (200):**
```json
{
  "sharedSecret": "JBSWY3DPEHPK3PXP",
  "qrCodeUri": "otpauth://totp/ONEX:user@example.com?secret=..."
}
```

### Verify 2FA

```http
POST /users/2fa/verify
Content-Type: application/json

{
  "email": "user@example.com",
  "code": "123456"
}
```

### Disable 2FA

```http
POST /users/2fa/disable
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "code": "123456"
}
```

---

## ?? User Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/users` | Get all users (paginated) | ? Required |
| `GET` | `/users/{id}` | Get user by ID | ? Required |
| `GET` | `/users/me` | Get current user | ? Required |
| `PUT` | `/users/{id}` | Update user | ? Required |
| `PUT` | `/users/me` | Update current user | ? Required |
| `POST` | `/users/{id}/activate` | Activate user | ? Required |
| `POST` | `/users/{id}/deactivate` | Deactivate user | ? Required |
| `POST` | `/users/{id}/suspend` | Suspend user | ? Required |
| `DELETE` | `/users/{id}` | Delete user | ? Required |

### Get Users (Paginated)

```http
GET /users?page=1&pageSize=10&search=john&sortBy=email&sortDescending=false
Authorization: Bearer {accessToken}
```

**Response (200):**
```json
{
  "items": [
    {
      "id": "...",
      "email": "john@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "status": "Active",
      "accountType": "Staff",
      "createdAt": "2024-12-01T00:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

### Update User

```http
PUT /users/{id}
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Smith",
  "accountType": "Staff"
}
```

---

## ?? User Profile

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/users/{id}/profile` | Get user profile | ? Required |
| `GET` | `/users/me/profile` | Get current user profile | ? Required |
| `PUT` | `/users/{id}/profile` | Update user profile | ? Required |
| `PUT` | `/users/me/profile` | Update current user profile | ? Required |

### Get Profile

```http
GET /users/me/profile
Authorization: Bearer {accessToken}
```

**Response (200):**
```json
{
  "userId": "...",
  "phoneNumber": "+1234567890",
  "avatarUrl": "https://...",
  "jobTitle": "Developer",
  "department": "Engineering",
  "bio": "...",
  "linkedInUrl": "...",
  "skills": ["C#", ".NET", "Angular"]
}
```

---

## ?? User Sessions

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/users/{id}/sessions` | Get user sessions | ? Required |
| `GET` | `/users/me/sessions` | Get current user sessions | ? Required |
| `DELETE` | `/users/{id}/sessions/{sessionId}` | Revoke session | ? Required |
| `DELETE` | `/users/{id}/sessions` | Revoke all sessions | ? Required |

---

## ?? Role Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/roles` | Get all roles | ? Required |
| `GET` | `/roles/{id}` | Get role by ID | ? Required |
| `POST` | `/roles` | Create role | ? Required |
| `PUT` | `/roles/{id}` | Update role | ? Required |
| `DELETE` | `/roles/{id}` | Delete role | ? Required |
| `POST` | `/roles/{id}/activate` | Activate role | ? Required |
| `POST` | `/roles/{id}/deactivate` | Deactivate role | ? Required |
| `GET` | `/roles/{id}/permissions` | Get role permissions | ? Required |
| `PUT` | `/roles/{id}/permissions` | Update role permissions | ? Required |

### Create Role

```http
POST /roles
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "name": "Project Manager",
  "description": "Can manage projects",
  "canViewSensitiveData": false
}
```

### Update Role Permissions

```http
PUT /roles/{id}/permissions
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "permissionIds": [
    "perm-id-1",
    "perm-id-2"
  ]
}
```

---

## ?? Permissions

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/permissions` | Get all permissions | ? Required |
| `GET` | `/users/me/permissions` | Get current user permissions | ? Required |

---

## ?? User Roles

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/users/{id}/roles` | Get user roles | ? Required |
| `POST` | `/users/{id}/roles` | Assign role to user | ? Required |
| `DELETE` | `/users/{id}/roles/{roleId}` | Remove role from user | ? Required |

---

## ?? Account Management

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/accounts` | Get all accounts | ? Required |
| `GET` | `/accounts/{id}` | Get account by ID | ? Required |
| `POST` | `/accounts` | Create account | ? Required |
| `PUT` | `/accounts/{id}` | Update account | ? Required |
| `DELETE` | `/accounts/{id}` | Delete account | ? Required |
| `POST` | `/accounts/{id}/activate` | Activate account | ? Required |
| `POST` | `/accounts/{id}/deactivate` | Deactivate account | ? Required |

### Create Account

```http
POST /accounts
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "name": "Acme Corporation",
  "industry": "Technology",
  "website": "https://acme.com",
  "phone": "+1234567890",
  "address": "123 Main St",
  "taxNumber": "123456789"
}
```

---

## ?? Account Contacts

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/accounts/{id}/contacts` | Get account contacts | ? Required |
| `POST` | `/accounts/{id}/contacts` | Add contact directly | ? Required |
| `PUT` | `/accounts/{id}/contacts/{contactId}` | Update contact | ? Required |
| `DELETE` | `/accounts/{id}/contacts/{contactId}` | Remove contact | ? Required |
| `PUT` | `/accounts/{id}/contacts/{contactId}/permissions` | Update permissions | ? Required |

---

## ?? Contact Invitations

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/accounts/{id}/invitations` | Send invitation | ? Required |
| `GET` | `/accounts/{id}/invitations` | Get pending invitations | ? Required |
| `POST` | `/accounts/{id}/invitations/{invId}/resend` | Resend invitation | ? Required |
| `DELETE` | `/accounts/{id}/invitations/{invId}` | Cancel invitation | ? Required |
| `POST` | `/invitations/{invId}/accept` | Accept invitation | ?? Public |

### Send Invitation

```http
POST /accounts/{id}/invitations
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "email": "contact@example.com",
  "firstName": "Jane",
  "lastName": "Smith",
  "role": "Product Owner",
  "expirationDays": 7
}
```

### Accept Invitation

```http
POST /invitations/{invitationId}/accept
Content-Type: application/json

{
  "token": "invitation-token-from-email",
  "password": "SecurePassword123!"
}
```

---

## ?? Audit Logs

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/audit-logs` | Get all audit logs | ? Required |
| `GET` | `/audit-logs/{id}` | Get audit log by ID | ? Required |
| `GET` | `/users/{id}/audit-logs` | Get user audit logs | ? Required |
| `GET` | `/users/me/audit-logs` | Get current user audit logs | ? Required |

### Get Audit Logs

```http
GET /audit-logs?page=1&pageSize=20&action=Login&startDate=2024-12-01
Authorization: Bearer {accessToken}
```

---

## ?? Health Check

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/health` | Health check | ?? Public |

---

## ?? Pagination

All list endpoints support pagination with these query parameters:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | int | 1 | Page number |
| `pageSize` | int | 10 | Items per page (max 100) |
| `search` | string | null | Search term |
| `sortBy` | string | varies | Field to sort by |
| `sortDescending` | bool | false | Sort direction |

**Response Format:**
```json
{
  "items": [...],
  "page": 1,
  "pageSize": 10,
  "totalCount": 100,
  "totalPages": 10,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## ? Error Responses

All errors follow RFC 7807 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "User.NotFound",
  "status": 404,
  "detail": "The user with Id = 'xxx' was not found",
  "traceId": "00-abc123-def456-00"
}
```

### Common Status Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request (Validation Error) |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 409 | Conflict |
| 500 | Internal Server Error |

---

## ?? JWT Token Claims

| Claim | Description |
|-------|-------------|
| `sub` | Domain User ID |
| `identityUserId` | Identity User ID |
| `email` | User email |
| `role` | User roles (array) |
| `permissions` | User permissions (array) |
| `canViewSensitiveData` | Can view sensitive data |
| `securityStamp` | Security stamp for invalidation |
| `exp` | Expiration time |
| `iss` | Issuer |
| `aud` | Audience |

---

## ?? Notes

1. **Token Expiration:** Access tokens expire in 20 minutes
2. **Refresh Token:** Valid for 7 days, rotated on each use
3. **Password Reset Token:** Expires in 20 minutes, one-time use
4. **Invitation Token:** Expires in 7 days (configurable), one-time use
5. **Security Stamp:** Changed on password change, logout, permission change
