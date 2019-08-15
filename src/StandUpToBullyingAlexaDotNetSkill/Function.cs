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
        string textToSpeak = null;
        bool shouldEndSession = false;

        private void dumpState(String message, ILambdaLogger logger) {
            var logString = String.Format("{0} BAB - state vector: {1}:{2}:{3}:{4}:{5}", 
                message, introStatus, practice1Started, practice1Started, practice2Started, practice3Started, followUp);
            logger.LogLine(logString);
        }
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;
            log.LogLine($"BAB - Skill Request Object:");
            log.LogLine(JsonConvert.SerializeObject(input));

            dumpState("BAB - Entering FunctionHandler", log);

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"BAB - Default LaunchRequest made: 'Alexa, Stand up to a bully");
                
                textToSpeak = "Hi, Brian. Welcome to Comforting Comebacks, " +
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
				var intentName = intentRequest.Intent.Name;

				log.LogLine($"Intent Request entered, type is {intentName}");

				ProcessIntentRequest(log, intentName);
			}

			dumpState("BAB - Exiting FunctionHandler", log);

            innerResponse = new PlainTextOutputSpeech();
            (innerResponse as PlainTextOutputSpeech).Text = textToSpeak;

            response.Response.ShouldEndSession = shouldEndSession;

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            log.LogLine($"BAB - Skill Response Object...");
            log.LogLine(JsonConvert.SerializeObject(response));
            log.LogLine("");
            return response;
        }

		public void ProcessIntentRequest(ILambdaLogger log, string intentName)
		{
			if (practice1Started == false && practice2Started == false && practice3Started == false && introStatus == false)
			{
				switch (intentName)
				{
					case "AMAZON.CancelIntent":
						log.LogLine($"BAB - AMAZON.CancelIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						shouldEndSession = true;
						break;
					case "StopIntent":
						log.LogLine($"BAB - AMAZON.StopIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						introStatus = false;
						practice1Started = false;
						practice2Started = false;
						practice3Started = false;
						followUp = 0;
						shouldEndSession = true;
						break;
					case "AMAZON.HelpIntent":
						log.LogLine($"BAB - AMAZON.HelpIntent: send HelpMessage");

						textToSpeak = "add later";
						break;
					case "ContinueIntent":
						log.LogLine($"BAB - choose to continue");

						textToSpeak = "The following comeback works best when you say" +
							" it calmly with a hint of sarcasm, then walk away.  When I say hey you, respond immediately " +
							"by saying the following phrase.  Talk to the hand!  Say Start to begin practicing If you need to hear this again, say repeat";
						introStatus = true;
						break;
					default:
						log.LogLine($"BAB - Unknown intent: " + intentName);
						dumpState("yes/no unknown path here", log);

						textToSpeak = "Please say continue , or if you dont want to continue just say stop to exit";
						break;
				}
			}
			else if (practice1Started == false && practice2Started == false && practice3Started == false && introStatus == true)
			{
				if (followUp == 0)
				{
					switch (intentName)
					{
						case "AMAZON.CancelIntent":
							log.LogLine($"BAB - AMAZON.CancelIntent: send StopMessage");

							textToSpeak = "Goodbye!";
							shouldEndSession = true;
							break;
						case "StopIntent":
							log.LogLine($"BAB - AMAZON.StopIntent: send StopMessage");

							textToSpeak = "Goodbye!";
							introStatus = false;
							practice1Started = false;
							practice2Started = false;
							practice3Started = false;
							followUp = 0;
							shouldEndSession = true;
							break;
						case "AMAZON.HelpIntent":
							log.LogLine($"BAB - AMAZON.HelpIntent: send HelpMessage");

							textToSpeak = "add later";
							break;
						case "RepeatIntent":
							log.LogLine($"BAB - phrases repeated");

							textToSpeak = "The phrase is Talk to the hand. " +
							" Would you like to practice?";
							phrase = "talk to the hand";
							followUp++;
							break;
						case "OpOneIntent":
							log.LogLine($"BAB - phrase 1 choosen");

							textToSpeak = "Your phrase is talk to the hand. Would you like to practice?";
							phrase = "talk to the hand";
							followUp++;
							break;
						default:
							log.LogLine($"BAB - Unknown intent: " + intentName);

							textToSpeak = "Unknown, remember to add later";
							break;
					}
				}
				else
				{
					switch (intentName)
					{
						case "YesIntent":
							log.LogLine($"BAB - Answer Yes");

							textToSpeak = "Here we go. 3! 2! 1! Hey you";
							practice1Started = true;
							break;
						case "NoIntent":
							log.LogLine($"BAB - Answer no");

							textToSpeak = "If you would like to hear the phrase again say repeat. If you are done" +
								" standing up to bullying then say exit or stop";
							followUp = 0;
							practice1Started = false;
							break;
						case "StopIntent":
							log.LogLine($"BAB - AMAZON.StopIntent: send StopMessage");

							textToSpeak = "Goodbye!";
							introStatus = false;
							practice1Started = false;
							practice2Started = false;
							practice3Started = false;
							followUp = 0;
							shouldEndSession = true;
							break;
						default:
							log.LogLine($"BAB - Unknown intent: " + intentName);

							textToSpeak = "Please answer yes or no";
							break;
					}
				}
			}
			else if (practice1Started == true)
			{
				switch (intentName)
				{
					case "AMAZON.CancelIntent":
						log.LogLine($"BAB - AMAZON.CancelIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						shouldEndSession = true;
						break;
					case "StopIntent":
						log.LogLine($"BAB - AMAZON.StopIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						introStatus = false;
						practice1Started = false;
						practice2Started = false;
						practice3Started = false;
						followUp = 0;
						shouldEndSession = true;
						break;
					case "AMAZON.HelpIntent":
						log.LogLine($"BAB - AMAZON.HelpIntent: send HelpMessage");

						textToSpeak = "Your phrase is " + phrase + "  " +
							"Lets try again! 3! 2! 1! Hey you";
						break;
					case "OpOneIntent":
						log.LogLine($"BAB - phrase 1 choosen");

						textToSpeak = "Good job. Let's try it again.  Remember" +
								" to speak calmly with a hint of sarcasm. 3! 2! 1! Hey you";
						practice1Started = false;
						practice2Started = true;
						break;
					default:
						log.LogLine($"BAB - Unknown intent: " + intentName);

						textToSpeak = "Good try! Listen to the phrase again, " + phrase;
						break;
				}
			}
			else if (practice2Started == true)
			{
				switch (intentName)
				{
					case "AMAZON.CancelIntent":
						log.LogLine($"BAB - AMAZON.CancelIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						shouldEndSession = true;
						break;
					case "StopIntent":
						log.LogLine($"BAB - AMAZON.StopIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						introStatus = false;
						practice1Started = false;
						practice2Started = false;
						practice3Started = false;
						followUp = 0;
						shouldEndSession = true;
						break;
					case "AMAZON.HelpIntent":
						log.LogLine($"BAB - AMAZON.HelpIntent: send HelpMessage");

						textToSpeak = "Your phrase is " + phrase + "  " +
							"Lets try again! 3! 2! 1! Hey you";
						break;
					case "OpOneIntent":
						log.LogLine($"BAB - phrase 1 choosen");

						textToSpeak = "Great. Let's try one more time." +
							"  Remember to make eye contact as you speak, then practice walking away when you're done" +
							" speaking. 3! 2! 1! Hey you";
						practice2Started = false;
						practice3Started = true;
						break;
					default:
						log.LogLine($"BAB - Unknown intent: " + intentName);

						textToSpeak = "Good try! Listen to the phrase again, " + phrase;
						break;
				}
			}
			else if (practice3Started == true)
			{
				switch (intentName)
				{
					case "AMAZON.CancelIntent":
						log.LogLine($"BAB - AMAZON.CancelIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						shouldEndSession = true;
						break;
					case "StopIntent":
						log.LogLine($"BAB - AMAZON.StopIntent: send StopMessage");

						textToSpeak = "Goodbye!";
						introStatus = false;
						practice1Started = false;
						practice2Started = false;
						practice3Started = false;
						followUp = 0;
						shouldEndSession = true;
						break;
					case "AMAZON.HelpIntent":
						log.LogLine($"BAB - AMAZON.HelpIntent: send HelpMessage");

						textToSpeak = "Your phrase is " + phrase + "  " +
							"Lets try again! 3! 2! 1! Hey you";
						break;
					case "OpOneIntent":
						log.LogLine($"BAB - phrase 1 choosen");

						textToSpeak = "Nice job. Would you like to practice another" +
							" phrase?";
						break;
					case "YesIntent":
						log.LogLine($"BAB - said yes");

						textToSpeak = "Okay just say, continue, to start over";
						practice3Started = false;
						introStatus = false;
						break;
					case "NoIntent":
						log.LogLine($"BAB - said no");

						textToSpeak = "Thanks for using Comforting Comebacks. Be sure" +
							" to play every week for new phrases and techniques.  Goodbye!";
						introStatus = false;
						practice1Started = false;
						practice2Started = false;
						practice3Started = false;
						followUp = 0;
						shouldEndSession = true;
						break;
					default:
						log.LogLine($"BAB - Unknown intent: " + intentName);

						textToSpeak = "Good try! Listen to the phrase again, " + phrase;
						break;
				}
			}
		}
	}
}
