# Hello

<style>  
  #inheritance > span:nth-child(2) {
    margin-left: 8px;
  }

  #inheritance > span:nth-child(3) {
    margin-left: 16px;
  }

  #inheritance > span:nth-child(4) {
    margin-left: 24px;
  }

  #inheritance > span::before {
    content: '↳ ';
  }

  #inheritance > span::after {
    content: '\A';
    white-space: pre;
  }
</style>

Inheritance:

<div id='inheritance'>
<span>Object</span>
<span>Another</span>
<span>AndYetAnother</span>
<span>GodHelpMe</span>
</div>
