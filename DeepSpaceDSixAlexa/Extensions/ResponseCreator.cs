using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace DeepSpaceDSixAlexa.Extensions
{
    public static class ResponseCreator
    {
        public static SkillResponse Ask(string message, Reprompt reprompt, Session session)
        {
            // converts plain message into ssmls
            var speech = new SsmlOutputSpeech();
            speech.Ssml = $"<speak> {message} </speak>";

            return ResponseBuilder.Ask(speech, reprompt, session);
        }

        public static SkillResponse Ask(string message, string reprompt, Session session)
        {
            return Ask(message, RepromptBuilder.Create(reprompt), session);
        }

        public static SkillResponse Ask(string message, Session session)
        {
            return Ask(message, RepromptBuilder.Create(message), session);
        }
    }
}
