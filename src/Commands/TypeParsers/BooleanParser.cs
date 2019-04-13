﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gommon;
using Qmmands;

namespace DepressedBot.Commands.TypeParsers
{
    public sealed class BooleanParser : TypeParser<bool>
    {
        private readonly string[] _matchingTrueValues =
        {
            "true", "y", "yes", "ye", "yep", "yeah", "sure", "affirmative", "yar", "aff", "ya", "da", "yas", "enable",
            "yip",
            "positive", "1"
        };

        private readonly string[] _matchingFalseValues =
        {
            "false", "n", "no", "nah", "na", "nej", "nope", "nop", "neg", "negatory", "disable", "nay", "negative", "0"
        };

        public override Task<TypeParserResult<bool>> ParseAsync(
            Parameter param,
            string value,
            ICommandContext context,
            IServiceProvider provider)
        {
            if (_matchingTrueValues.ContainsIgnoreCase(value))
                return Task.FromResult(TypeParserResult<bool>.Successful(true));

            if (_matchingFalseValues.ContainsIgnoreCase(value))
                return Task.FromResult(TypeParserResult<bool>.Successful(false));

            if (bool.TryParse(value, out var result))
                return Task.FromResult(TypeParserResult<bool>.Successful(result));

            return Task.FromResult(
                TypeParserResult<bool>.Unsuccessful("Failed to parse a boolean (true/false) value."));
        }
    }
}
