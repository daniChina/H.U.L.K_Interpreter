public enum TokenType
{
    Identifier,

    // KeyWords
    LetKeyWord,

    InKeyWord,

    FunctionKeyWord,

    IfKeyWord, 
    
    ElseKeyWord, 
    
    // Data Types
    String,

    Number,

    Bool,

    // Arithmetic Operators
    Addition,

    Substraction,

    Multiplication,

    Division,

    Power,

    Modulus,

    // Boolean Operators
    Equals,

    Concat,

    And,

    Or,

    EqualsTo,

    LessOrEquals,

    LessThan,

    GreaterOrEquals,

    GreaterThan,

    NotEquals,

    Not,

    // Punctuators
    LeftParenthesis,

    RightParenthesis,

    Comma,

    Colon,

    Semicolon,

    FullStop,

    Quote,

    Arrow,

    // Utility 
    EndOfLine,

    Temporal,

    Unknown,

}