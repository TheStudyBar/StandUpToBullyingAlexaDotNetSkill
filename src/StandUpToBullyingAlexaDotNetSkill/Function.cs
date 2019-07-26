using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization;
using Alexa.NET.Response;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace StandUpToBullyingAlexaDotNetSkill
{
    public class Function
    {
        string phrase = "";
        bool introStatus = false;
        bool practice1Started = false;
        bool practice2Started = false;
        bool practice3Started = false;
        int followUp = 0;

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;
            log.LogLine($"Skill Request Object:");
            log.LogLine(JsonConvert.SerializeObject(input));


            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made: 'Alexa, Stand up to a bully");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Hi, Brian. Welcome to Comforting Comebacks, " +
                    "the number one guide for standing up to bullies effectively. " +
                    "I will simulate a bully and help you practice the best ways to respond. " +
                    "Say continue to begin learning.";
                phrase = "";
                introStatus = false;
                practice1Started = false;
                practice2Started = false;
                practice3Started = false;
                followUp = 0;

            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                if (practice1Started == false && practice2Started == false && practice3Started == false && introStatus == false)
                {
                    switch (intentRequest.Intent.Name)
                    {
                        case "AMAZON.CancelIntent":
                            log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            response.Response.ShouldEndSession = true;
                            break;
                        case "StopIntent":
                            log.LogLine($"AMAZON.StopIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            introStatus = false;
                            practice1Started = false;
                            practice2Started = false;
                            practice3Started = false;
                            followUp = 0;
                            response.Response.ShouldEndSession = true;
                            break;
                        case "AMAZON.HelpIntent":
                            log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "add later";
                            break;
                        case "ContinueIntent":
                            log.LogLine($"choose to continue");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "The following comebacks work best when you say" +
                                " them calmy with a hint of sarcasm, then walk away.  When I say hey, you, respond immediately " +
                                "by saying of the three following phrases.  Option 1, talk to the hand.  Option 2, Wow! Now I know why " +
                                "everyone says that stuff about you.  Option 3, Do you think you could star in the next Mean Girls movie?  " +
                                "Pick one phrase and say it back now.  If you need to hear them again, say repeat";
                            introStatus = true;
                            break;
                        default:
                            log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Please say continue , or if you dont want to continue just say stop to exit";
                            break;
                    }
                }
                else if (practice1Started == false && practice2Started == false && practice3Started == false && introStatus == true)
                {
                    if (followUp == 0)
                    {
                        switch (intentRequest.Intent.Name)
                        {
                            case "AMAZON.CancelIntent":
                                log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                                response.Response.ShouldEndSession = true;
                                break;
                            case "StopIntent":
                                log.LogLine($"AMAZON.StopIntent: send StopMessage");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                                introStatus = false;
                                practice1Started = false;
                                practice2Started = false;
                                practice3Started = false;
                                followUp = 0;
                                response.Response.ShouldEndSession = true;
                                break;
                            case "AMAZON.HelpIntent":
                                log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "add later";
                                break;
                            case "RepeatIntent":
                                log.LogLine($"phrases repeated");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Option 1, talk to the hand.  Option 2, Wow! Now I know why " +
                                    "everyone says that stuff about you.  Option 3, Do you think you could star in the next Mean Girls movie?!" +
                                    "  Which phrase would you like to practice?";
                                break;
                            case "OpOneIntent":
                                log.LogLine($"phrase 1 choosen");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "You have chosen, talk to the hand. Are you ready to practice?";
                                phrase = "talk to the hand";
                                followUp++;
                                break;
                            case "OpTwoIntent":
                                log.LogLine($"phrase 2 choosen");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "You have chosen, Wow! Now I know why " +
                                    "everyone says that stuff about you. Are you ready to practice?";
                                phrase = "Wow! Now I know why everyone says that stuff about you.";
                                followUp++;
                                break;
                            case "OpThreeIntent":
                                log.LogLine($"phrase 3 choosen");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "You have chosen, Do you think you could star in the next" +
                                    " Mean Girls movie?. Are you ready to practice?";
                                phrase = "Do you think you could star in the next Mean Girls movie?";
                                followUp++;
                                break;
                            default:
                                log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Unknown, remember to add later";
                                break;
                        }
                    }
                    else
                    {
                        switch (intentRequest.Intent.Name)
                        {
                            case "YesIntent":
                                log.LogLine($"Answer Yes");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Here we go. 3! 2! 1! Hey, you";
                                practice1Started = true;
                                break;
                            case "NoIntent":
                                log.LogLine($"Answer no");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "If you would like to hear the phrases again say repeat. If you are done" +
                                    " standing up to bullying then say exit or stop";
                                followUp = 0;
                                practice1Started = false;
                                break;
                            case "StopIntent":
                                log.LogLine($"AMAZON.StopIntent: send StopMessage");
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                                introStatus = false;
                                practice1Started = false;
                                practice2Started = false;
                                practice3Started = false;
                                followUp = 0;
                                response.Response.ShouldEndSession = true;
                                break;
                            default:
                                log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                                innerResponse = new PlainTextOutputSpeech();
                                (innerResponse as PlainTextOutputSpeech).Text = "Please answer yes or no";
                                break;
                        }
                    }
                }
                else if (practice1Started == true)
                {
                    switch (intentRequest.Intent.Name)
                    {
                        case "AMAZON.CancelIntent":
                            log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            response.Response.ShouldEndSession = true;
                            break;
                        case "StopIntent":
                            log.LogLine($"AMAZON.StopIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            introStatus = false;
                            practice1Started = false;
                            practice2Started = false;
                            practice3Started = false;
                            followUp = 0;
                            response.Response.ShouldEndSession = true;
                            break;
                        case "AMAZON.HelpIntent":
                            log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "The comeback you chose was " + phrase + "  " +
                                "Lets try again! 3! 2! 1! Hey, you";
                            break;
                        case "OpOneIntent":
                            log.LogLine($"phrase 1 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Good job. Let's try it again.  Remember" +
                                    " to speak calmly with a hint of sarcasm. 3! 2! 1! Hey, you";
                            practice1Started = false;
                            practice2Started = true;
                            break;
                        case "OpTwoIntent":
                            log.LogLine($"phrase 2 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Good job. Let's try it again.  Remember" +
                                " to speak calmly with a hint of sarcasm. 3! 2! 1! Hey, you";
                            practice1Started = false;
                            practice2Started = true;
                            break;
                        case "OpThreeIntent":
                            log.LogLine($"phrase 3 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Good job. Let's try it again.  Remember" +
                                " to speak calmly with a hint of sarcasm. 3! 2! 1! Hey, you";
                            practice1Started = false;
                            practice2Started = true;
                            break;
                        default:
                            log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Good try! Listen to the phrase again, " + phrase;
                            break;
                    }
                }
                else if (practice2Started == true)
                {
                    switch (intentRequest.Intent.Name)
                    {
                        case "AMAZON.CancelIntent":
                            log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            response.Response.ShouldEndSession = true;
                            break;
                        case "StopIntent":
                            log.LogLine($"AMAZON.StopIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            introStatus = false;
                            practice1Started = false;
                            practice2Started = false;
                            practice3Started = false;
                            followUp = 0;
                            response.Response.ShouldEndSession = true;
                            break;
                        case "AMAZON.HelpIntent":
                            log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "The comeback you chose was " + phrase + "  " +
                                "Lets try again! 3! 2! 1! Hey, you";
                            break;
                        case "OpOneIntent":
                            log.LogLine($"phrase 1 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Great. Let's try one more time." +
                                "  Remember to make eye contact as you speak, then practice walking away when you're done" +
                                " speaking. 3! 2! 1! Hey, you";
                            practice2Started = false;
                            practice3Started = true;
                            break;
                        case "OpTwoIntent":
                            log.LogLine($"phrase 2 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Great. Let's try one more time." +
                                "  Remember to make eye contact as you speak, then practice walking away when you're done" +
                                " speaking. 3! 2! 1! Hey, you";
                            practice2Started = false;
                            practice3Started = true;
                            break;
                        case "OpThreeIntent":
                            log.LogLine($"phrase 3 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Great. Let's try one more time." +
                                "  Remember to make eye contact as you speak, then practice walking away when you're done" +
                                " speaking. 3! 2! 1! Hey, you";
                            practice2Started = false;
                            practice3Started = true;
                            break;
                        default:
                            log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Good try! Listen to the phrase again, " + phrase;
                            break;
                    }
                }
                else if (practice3Started == true)
                {
                    switch (intentRequest.Intent.Name)
                    {
                        case "AMAZON.CancelIntent":
                            log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            response.Response.ShouldEndSession = true;
                            break;
                        case "StopIntent":
                            log.LogLine($"AMAZON.StopIntent: send StopMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Goodbye!";
                            introStatus = false;
                            practice1Started = false;
                            practice2Started = false;
                            practice3Started = false;
                            followUp = 0;
                            response.Response.ShouldEndSession = true;
                            break;
                        case "AMAZON.HelpIntent":
                            log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "The comeback you chose was " + phrase + "  " +
                                "Lets try again! 3! 2! 1! Hey, you";
                            break;
                        case "OpOneIntent":
                            log.LogLine($"phrase 1 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Nice job. Would you like to practice another" +
                                " phrase?";
                            break;
                        case "OpTwoIntent":
                            log.LogLine($"phrase 2 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Nice job. Would you like to practice another" +
                                " phrase?";
                            break;
                        case "OpThreeIntent":
                            log.LogLine($"phrase 3 choosen");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Nice job. Would you like to practice another" +
                                " phrase?";
                            break;
                        case "YesIntent":
                            log.LogLine($"said yes");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Okay just say, continue, to start over";
                            practice3Started = false;
                            introStatus = false;
                            break;
                        case "NoIntent":
                            log.LogLine($"said no");
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Thanks for using Comforting Comebacks. Be sure" +
                                " to play every week for new phrases and techniques.  Goodbye!";
                            introStatus = false;
                            practice1Started = false;
                            practice2Started = false;
                            practice3Started = false;
                            followUp = 0;
                            response.Response.ShouldEndSession = true;
                            break;
                        default:
                            log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                            innerResponse = new PlainTextOutputSpeech();
                            (innerResponse as PlainTextOutputSpeech).Text = "Good try! Listen to the phrase again, " + phrase;
                            break;
                    }
                }
            }

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            log.LogLine($"Skill Response Object...");
            log.LogLine(JsonConvert.SerializeObject(response));
            return response;
        }
    }
}
