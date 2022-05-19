parser grammar Xml;

options {
  tokenVocab=XmlLexer;
}

string:
    QUOTED_STRING
    | DOUBLE_QUOTED_STRING;

attribute: IDENTIFIER EQUAL string;

prolog: START_TAG QUESTION_MARK XML (attribute)* QUESTION_MARK END_TAG;

element: START_TAG IDENTIFIER (attribute)* END_TAG;
endElement: START_TAG SLASH IDENTIFIER END_TAG;
emptyElement: START_TAG IDENTIFIER (attribute)* SLASH END_TAG;
textElement: TEXT;

node:
    element #Begin
    | endElement #End
    | emptyElement #Empty
    | textElement #Text
    ;

unit: prolog? node+ EOF;