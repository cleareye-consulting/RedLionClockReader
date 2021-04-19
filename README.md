# RedLionClockReader
Windows service to retrieve readings from a Red Lion clock and post the result to a URL

Requires the RS232 package in a separate repo (has a project-level reference).

The RedLionFakeClock application can be used in conjunction with a COM port emulator like com0com to test reading.

Requires an endpoint to post the reading to. The message posted to the endpoint is a JSON structure containing deviceId and value. Device ID is configured in appsettings.
