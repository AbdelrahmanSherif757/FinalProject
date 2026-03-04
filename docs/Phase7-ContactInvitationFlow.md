# ONEX API Documentation - Phase 7: Contact Invitation Flow

## ?? Overview

This document covers the complete Contact Invitation Flow implemented in the ONEX system, including all endpoints, flows, and security considerations.

---

## ??? Architecture

```
???????????????????????????????????????????????????????????????????????????????
?                              ONEX SYSTEM                                     ?
???????????????????????????????????????????????????????????????????????????????
?                                                                              ?
?  ??ž?? STAFF (Your Company Employees)          ?? ACCOUNTS (Client Companies) ?
?  ??? Developer                                ??? ?? CONTACTS (Client Users) ?
?  ??? Project Manager                              ??? CEO (Primary)          ?
?  ??? Designer                                     ??? CTO                     ?
?  ??? Admin                                        ??? Product Owner           ?
?  ??? Uses: Role + Permission                      ??? Uses: ContactPermissions?
?                                                                              ?
???????????????????????????????????????????????????????????????????????????????
```

---

## ?? Contact Invitation Flow

### Complete Flow Diagram

```
???????????????????????????????????????????????????????????????????????????????
?  1. INVITE: Admin/Primary Contact sends invitation                           ?
?     POST /accounts/{accountId}/invitations                                   ?
?     ? Creates User (if new) with AccountType.ClientContact                  ?
?     ? Creates AccountContact with MINIMAL permissions                        ?
?     ? Generates secure invitation token (SHA256 hashed)                     ?
?     ? Sends invitation email via MailKit                                    ?
???????????????????????????????????????????????????????????????????????????????
                                    ?
                                    ?
???????????????????????????????????????????????????????????????????????????????
?  2. EMAIL: Recipient receives invitation                                     ?
?     ? Professional HTML email with "Accept Invitation" button               ?
?     ? Link: {AppBaseUrl}/accept-invitation?token={token}&id={invitationId}  ?
?     ? Expires in 7 days (configurable)                                      ?
???????????????????????????????????????????????????????????????????????????????
                                    ?
                                    ?
???????????????????????????????????????????????????????????????????????????????
?  3. ACCEPT: User accepts invitation                                          ?
?     POST /invitations/{invitationId}/accept                                  ?
?     ? Validates token (one-time use)                                        ?
?     ? Creates Identity user (if new, with password)                         ?
?     ? Activates AccountContact                                              ?
?     ? Clears invitation token                                               ?
???????????????????????????????????????????????????????????????????????????????
                                    ?
                                    ?
???????????????????????????????????????????????????????????????????????????????
?  4. SET PERMISSIONS: Primary Contact configures access                       ?
?     PUT /accounts/{accountId}/contacts/{contactId}/permissions              ?
?     ? Sets specific permissions for the contact                             ?
?     ? Contact can now access allowed resources                              ?
???????????????????????????????????????????????????????????????????????????????
                                    ?
                                    ?
???????????????????????????????????????????????????????????????????????????????
?  5. LOGIN: User can now login and access account                             ?
?     POST /users/login                                                        ?
?     ? Gets JWT tokens                                                        ?
?     ? Access resources based on ContactPermissions                          ?
???????????????????????????????????????????????????????????????????????????????
```

---

## ?? API Endpoints

### Invitation Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/accounts/{accountId}/invitations` | Send invitation | ? Required |
| `POST` | `/invitations/{invitationId}/accept` | Accept invitation | ?? Public |
| `POST` | `/accounts/{accountId}/invitations/{invitationId}/resend` | Resend invitation | ? Required |
| `DELETE` | `/accounts/{accountId}/invitations/{invitationId}` | Cancel invitation | ? Required |
| `GET` | `/accounts/{accountId}/invitations` | Get pending invitations | ? Required |

### Contact Management Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/accounts/{accountId}/contacts` | Get account contacts | ? Required |
| `POST` | `/accounts/{accountId}/contacts` | Add contact directly | ? Required |
| `PUT` | `/accounts/{accountId}/contacts/{contactId}` | Update contact | ? Required |
| `DELETE` | `/accounts/{accountId}/contacts/{contactId}` | Remove contact | ? Required |
| `PUT` | `/accounts/{accountId}/contacts/{contactId}/permissions` | Update permissions | ? Required |

### Account Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `GET` | `/accounts` | Get all accounts | ? Required |
| `POST` | `/accounts` | Create account | ? Required |
| `GET` | `/accounts/{id}` | Get account by ID | ? Required |
| `PUT` | `/accounts/{id}` | Update account | ? Required |
| `DELETE` | `/accounts/{id}` | Delete account | ? Required |
| `POST` | `/accounts/{id}/activate` | Activate account | ? Required |
| `POST` | `/accounts/{id}/deactivate` | Deactivate account | ? Required |

---

## ?? Endpoint Details

### 1. Send Invitation

**Endpoint:** `POST /accounts/{accountId}/invitations`

**Authorization:** Staff users or Primary Contact with `CanManageContacts` permission

**Request Body:**
```json
{
  "email": "contact@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Product Owner",
  "expirationDays": 7
}
```

**Response (201 Created):**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

**Notes:**
- Contact is created with **MINIMAL permissions** (no access)
- Primary Contact must set permissions after invitation is accepted
- Email is sent automatically via configured SMTP

---

### 2. Accept Invitation

**Endpoint:** `POST /invitations/{invitationId}/accept`

**Authorization:** Public (accessed via email link)

**Request Body:**
```json
{
  "token": "base64-encoded-token-from-email",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "message": "Invitation accepted successfully. You can now log in."
}
```

**Notes:**
- `password` is required only for **new users**
- Token is one-time use (cleared after acceptance)
- Token is validated against SHA256 hash

---

### 3. Resend Invitation

**Endpoint:** `POST /accounts/{accountId}/invitations/{invitationId}/resend`

**Authorization:** Staff users or Primary Contact with `CanManageContacts` permission

**Response (200 OK):**
```json
{
  "message": "Invitation resent successfully."
}
```

**Notes:**
- Generates a new token
- Resets expiration date
- Sends new email

---

### 4. Cancel Invitation

**Endpoint:** `DELETE /accounts/{accountId}/invitations/{invitationId}`

**Authorization:** Staff users or Primary Contact with `CanManageContacts` permission

**Response:** `204 No Content`

**Notes:**
- Soft deletes the invitation
- Contact record is deactivated
- Cannot cancel accepted invitations

---

### 5. Get Pending Invitations

**Endpoint:** `GET /accounts/{accountId}/invitations`

**Authorization:** Staff users or Primary Contact with `CanManageContacts` permission

**Response (200 OK):**
```json
[
  {
    "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "userId": "b2c3d4e5-f6a7-8901-bcde-f23456789012",
    "email": "contact@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Product Owner",
    "invitedAt": "2024-12-27T01:00:00Z",
    "expiresAt": "2025-01-03T01:00:00Z",
    "isExpired": false,
    "invitedByName": "Admin User"
  }
]
```

---

### 6. Update Contact Permissions

**Endpoint:** `PUT /accounts/{accountId}/contacts/{contactId}/permissions`

**Authorization:** Staff users or Primary Contact with `CanManageContacts` permission

**Request Body:**
```json
{
  "canCreateTickets": true,
  "canViewAllTickets": false,
  "canViewProjects": true,
  "canViewInvoices": false,
  "canViewContracts": false,
  "canViewFinancials": false,
  "canManageContacts": false,
  "canApproveDeliverables": false,
  "canDownloadFiles": true,
  "receiveNotifications": true
}
```

**Response:** `204 No Content`

**Notes:**
- Cannot modify Primary Contact's permissions (always full access)
- Must be called after invitation acceptance to grant access

---

## ?? Contact Permissions

### Permission Levels

| Level | Description | Use Case |
|-------|-------------|----------|
| `CreateMinimal()` | No permissions (notifications only) | New invitations |
| `CreateDefault()` | Basic access (tickets, projects, files) | Regular contacts |
| `CreateFull()` | All permissions | Primary Contact |

### Permission Details

| Permission | Description |
|------------|-------------|
| `CanCreateTickets` | Can create support tickets |
| `CanViewAllTickets` | Can view all account tickets (not just own) |
| `CanViewProjects` | Can view project progress and milestones |
| `CanViewInvoices` | Can view invoices and payment history |
| `CanViewContracts` | Can view and download contracts |
| `CanViewFinancials` | Can view financial reports and statements |
| `CanManageContacts` | Can add/remove other contacts |
| `CanApproveDeliverables` | Can approve or reject deliverables |
| `CanDownloadFiles` | Can download project files and assets |
| `ReceiveNotifications` | Receives email notifications |

---

## ?? Email Templates

### Invitation Email

**Subject:** `You're invited to join {AccountName} on ONEX`

**Features:**
- Professional HTML design with gradient header
- Clear call-to-action button
- Expiration warning
- Plain text fallback

---

## ?? Security Features

### Token Security

| Feature | Implementation |
|---------|----------------|
| Token Generation | 32 bytes random (RandomNumberGenerator) |
| Token Storage | SHA256 hash (never plain text) |
| Token Validation | Constant-time comparison |
| One-Time Use | Cleared after acceptance |
| Expiration | Configurable (default 7 days) |

### Authorization

| User Type | Can Invite | Can Manage |
|-----------|------------|------------|
| Staff (Admin) | ? Any account | ? Any account |
| Primary Contact | ? Own account | ? Own account |
| Contact with CanManageContacts | ? Own account | ? Own account |
| Regular Contact | ? | ? |

---

## ??? Database Schema

### AccountContact Table

```sql
CREATE TABLE account_contacts (
    id UUID PRIMARY KEY,
    account_id UUID NOT NULL REFERENCES accounts(id),
    user_id UUID NOT NULL REFERENCES users(id),
    is_primary_contact BOOLEAN NOT NULL DEFAULT FALSE,
    role VARCHAR(100),
    is_decision_maker BOOLEAN NOT NULL DEFAULT FALSE,
    invited_by UUID,
    invited_at TIMESTAMP,
    invite_expires_at TIMESTAMP,
    invitation_token_hash VARCHAR(256),
    is_invite_accepted BOOLEAN NOT NULL DEFAULT FALSE,
    accepted_at TIMESTAMP,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP,
    -- ContactPermissions (owned entity)
    can_create_tickets BOOLEAN NOT NULL DEFAULT FALSE,
    can_view_all_tickets BOOLEAN NOT NULL DEFAULT FALSE,
    can_view_projects BOOLEAN NOT NULL DEFAULT FALSE,
    can_view_invoices BOOLEAN NOT NULL DEFAULT FALSE,
    can_view_contracts BOOLEAN NOT NULL DEFAULT FALSE,
    can_view_financials BOOLEAN NOT NULL DEFAULT FALSE,
    can_manage_contacts BOOLEAN NOT NULL DEFAULT FALSE,
    can_approve_deliverables BOOLEAN NOT NULL DEFAULT FALSE,
    can_download_files BOOLEAN NOT NULL DEFAULT FALSE,
    receive_notifications BOOLEAN NOT NULL DEFAULT TRUE
);
```

---

## ?? Testing

### Test Script

Location: `tests/invitation-flow-test.ps1`

**Tests:**
1. ? Admin login
2. ? Create account
3. ? Send invitation (minimal permissions)
4. ? Check email in MailPit
5. ? Verify pending invitation
6. ? Accept invitation with password
7. ? Verify invitation removed from pending
8. ? User can login
9. ? User info accessible
10. ? User in account contacts

**Run Test:**
```powershell
.\tests\invitation-flow-test.ps1
```

---

## ?? File Structure

```
src/
??? Application/
?   ??? Accounts/
?       ??? InviteContact/
?       ?   ??? InviteContactCommand.cs
?       ?   ??? InviteContactCommandValidator.cs
?       ?   ??? InviteContactCommandHandler.cs
?       ??? AcceptInvitation/
?       ?   ??? AcceptInvitationCommand.cs
?       ?   ??? AcceptInvitationCommandValidator.cs
?       ?   ??? AcceptInvitationCommandHandler.cs
?       ??? ResendInvitation/
?       ?   ??? ResendInvitationCommand.cs
?       ?   ??? ResendInvitationCommandHandler.cs
?       ??? CancelInvitation/
?       ?   ??? CancelInvitationCommand.cs
?       ?   ??? CancelInvitationCommandHandler.cs
?       ??? GetPendingInvitations/
?       ?   ??? GetPendingInvitationsQuery.cs
?       ?   ??? GetPendingInvitationsQueryHandler.cs
?       ??? UpdateContactPermissions/
?           ??? UpdateContactPermissionsCommand.cs
?           ??? UpdateContactPermissionsCommandValidator.cs
?           ??? UpdateContactPermissionsCommandHandler.cs
??? Domain/
?   ??? Accounts/
?       ??? AccountContact.cs (updated with invitation methods)
?       ??? ContactPermissions.cs (added CreateMinimal)
?       ??? AccountContactErrors.cs (added new errors)
??? Infrastructure/
?   ??? Email/
?       ??? EmailService.cs (added SendContactInvitationAsync)
??? Web.Api/
    ??? Endpoints/
        ??? Accounts/
            ??? InviteContact.cs
            ??? AcceptInvitation.cs
            ??? ResendInvitation.cs
            ??? CancelInvitation.cs
            ??? GetPendingInvitations.cs
            ??? UpdateContactPermissions.cs
```

---

## ?? Configuration

### Email Settings (appsettings.json)

```json
{
  "Email": {
    "SmtpHost": "localhost",
    "SmtpPort": 1025,
    "UseSsl": false,
    "FromEmail": "noreply@onex.local",
    "FromName": "ONEX System",
    "AppBaseUrl": "http://localhost:3000",
    "EnableSending": true
  }
}
```

### Token Settings (appsettings.json)

```json
{
  "Jwt": {
    "AccessTokenExpirationMinutes": 20,
    "RefreshTokenExpirationDays": 7
  },
  "TokenCleanup": {
    "Enabled": true,
    "IntervalMinutes": 60
  }
}
```

---

## ?? Deployment Notes

### MailPit (Development)

```yaml
# docker-compose.yml
mailpit:
  image: axllent/mailpit:latest
  ports:
    - "8025:8025"  # Web UI
    - "1025:1025"  # SMTP
```

### Production Email

For production, configure a real SMTP provider:
- SendGrid
- AWS SES
- Mailgun
- etc.

```json
{
  "Email": {
    "SmtpHost": "smtp.sendgrid.net",
    "SmtpPort": 587,
    "UseSsl": true,
    "Username": "apikey",
    "Password": "YOUR_API_KEY",
    "FromEmail": "noreply@yourdomain.com"
  }
}
```

---

## ?? Summary

| Feature | Status |
|---------|--------|
| Invite Contact via Email | ? Complete |
| Accept Invitation | ? Complete |
| Resend Invitation | ? Complete |
| Cancel Invitation | ? Complete |
| Get Pending Invitations | ? Complete |
| Update Contact Permissions | ? Complete |
| Minimal Permissions on Invite | ? Complete |
| One-Time Token Use | ? Complete |
| Token Expiration | ? Complete |
| Staff Authorization | ? Complete |
| Email Templates | ? Complete |
| Integration Tests | ? Complete |
