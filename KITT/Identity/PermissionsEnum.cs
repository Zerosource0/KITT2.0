using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KITTBackend.Identity
{
    public enum PermissionsEnum
        {
            CanEditKeys,
            CanEditTranslations,
            CanDeleteKeys,
            CanCreateKeys,
            CanCreateLocales,
            CanUpload,
            CanDownload,
            CanAdmin
        }

}