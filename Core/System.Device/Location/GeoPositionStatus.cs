// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*=============================================================================
**
** Class: GeoPositionStatus
**
** Purpose: Represents a GeoPositionStatus object
**
=============================================================================*/

namespace System.Device.Location
{
    public enum GeoPositionStatus
    {
        Ready,          // Enabled
        Initializing,   // Working to acquire data
        NoData,         // We have access to sensors, but we cannnot resolve
        Disabled        // Location service disabled or access denied
    }
}
