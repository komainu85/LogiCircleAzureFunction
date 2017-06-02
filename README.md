# Logi Circle Control from Azure Function
Control Logi Circle via Azure Function

Add these App Settings in Azure
* CameraUrl1
* CameraUrl2
* LogiEmail
* LogiPassword
* LogiLoginUrl

# Use
Make a POST to the azure function url with the below setting the power to "on" or "off". Can be integrated with IFTTT to trigger the function.

```javascript
{
    "power": "on"
}
```
