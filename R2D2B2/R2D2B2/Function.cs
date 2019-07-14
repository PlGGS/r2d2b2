using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace R2D2B2
{
    public class Function
    {
        SkillResponse response = new SkillResponse();
        readonly string version = "1.0.0";
        Type skillRequestType = typeof(LaunchRequest); //Default type to launchRequest bc it will always be that first
        ILambdaLogger logger;

        public SkillResponse FunctionHandler (SkillRequest input, ILambdaContext context)
        {
            response.Response = new ResponseBody();
            response.Version = version;

            skillRequestType = input.GetRequestType();
            logger = context.Logger;

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                Speak($"Beep boop beep bop. I am R2D2B2 version {version}");
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                IntentRequest intentRequest = (IntentRequest)input.Request;

                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        Speak("Cancelled", false);
                        break;
                    case "AMAZON.HelpIntent":
                        Speak("Placeholder help message");
                        response.Response.ShouldEndSession = false;
                        break;
                    case "AMAZON.StopIntent":
                        Speak("Cancelled");
                        response.Response.ShouldEndSession = false;
                        break;
                    case "AMAZON.NavigateHomeIntent":
                        Speak("AMAZON.NavigateHomeIntent: send default message like: Hey R2");
                        response.Response.ShouldEndSession = false;
                        break;
                    case "RememberBryanIntent":
                        Speak("Hello people of America and possibly other countries! I am not British, but today, I'll be showing you <break time=\"2s\"/> a map." + SsmlAudioFile("https://bblake.info/assets/r2d2b2/never.mp3"));
                        response.Response.ShouldEndSession = false;
                        break;
                    case "RememberJoeIntent":
                        Speak("I love being called Joey!" + SsmlAudioFile("https://bblake.info/assets/r2d2b2/what-what-what-does-that-even-mean.mp3"));
                        response.Response.ShouldEndSession = false;
                        break;
                    case "RememberPersonIntent":
                        Speak(  "What the fuck did you just fucking say about me, you little bitch? I’ll have you know " +
                                "I graduated top of my class in the Navy Seals, and I’ve been involved in numerous secret raids on " +
                                "Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I’m the top " +
                                "sniper in the entire US armed forces.You are nothing to me but just another target.I will wipe you " +
                                "the fuck out with precision the likes of which has never been seen before on this Earth, mark my " +
                                "fucking words. You think you can get away with saying that shit to me over the Internet ? Think " +
                                "again, fucker.As we speak I am contacting my secret network of spies across the USA and your IP is " +
                                "being traced right now so you better prepare for the storm, maggot. The storm that wipes out the " +
                                "pathetic little thing you call your life.You’re fucking dead, kid.I can be anywhere, anytime, and " +
                                "I can kill you in over seven hundred ways, and that’s just with my bare hands. Not only am I " +
                                "extensively trained in unarmed combat, but I have access to the entire arsenal of the United " +
                                "States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face " +
                                "of the continent, you little shit.If only you could have known what unholy retribution your little " +
                                "“clever” comment was about to bring down upon you, maybe you would have held your fucking tongue. " +
                                "But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot. I will shit fury " +
                                "all over you and you will drown in it. You’re fucking dead, kiddo.");
                        break;
                    default:
                        Speak($"Unknown intent: {intentRequest.Intent.Name}");
                        break;
                }
            }

            return response;
        }

        void Speak (string ssml, bool log = true, bool endSession = false)
        {
            response.Response.ShouldEndSession = endSession;

            if (log)
            {
                logger.LogLine($"{skillRequestType.ToString()} at <say-as interpret-as=\"time\">{DateTime.Now}</say-as> P.M");
            }

            response.Response.OutputSpeech = new SsmlOutputSpeech()
            {
                Ssml = $"<speak>{ssml}</speak>"
            };
        }

        string SsmlAudioFile (string fileURL)
        {
            return $"<audio src=\"{fileURL}\"/>";
        }
    }
}
