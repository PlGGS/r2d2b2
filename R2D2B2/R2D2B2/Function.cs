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
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;


            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made: 'Alexa, launch R2D2B2");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Successfully launched R2";
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Cancelled";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.StopIntent":
                        log.LogLine($"AMAZON.StopIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Cancelled";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Placeholder help message";
                        break;
                    case "RememberPersonIntent":
                        log.LogLine($"GetFactIntent sent: send new fact");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "What the fuck did you just fucking say about me, you little bitch? I’ll have you know " +
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
                "But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot.I will shit fury " +
                "all over you and you will drown in it. You’re fucking dead, kiddo.";
                        break;
                    default:
                        log.LogLine($"Unknown intent: {intentRequest.Intent.Name}");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = $"Unknown intent: {intentRequest.Intent.Name}";
                        break;
                }
            }

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            return response;
        }
    }
}
