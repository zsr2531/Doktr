lexer grammar XmlLexer;

START_TAG: '<' -> pushMode(IN_TAG);
TEXT: (~[<])+;

fragment S: [ \t\r\n];
fragment ID: [A-Za-z_][A-Za-z0-9_]*;

mode IN_TAG;
    END_TAG: '>' -> popMode;
    XML: 'xml';
    QUOTE: '\'';
    DOUBLE_QUOTE: '"';
    EQUAL: '=';
    SLASH: '/';
    QUESTION_MARK: '?';
    QUOTED_STRING: QUOTE (~'\'' | ~'\n' | '\\\'')*? QUOTE;
    DOUBLE_QUOTED_STRING: DOUBLE_QUOTE (~'"' | ~'\n' | '\\"')*? DOUBLE_QUOTE;
    IDENTIFIER: ID;
    WS: S -> channel(HIDDEN);