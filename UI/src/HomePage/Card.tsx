import React from 'react'; // importing FunctionComponent

type CardProps = {
  title: string,
  paragraph: string
}

export const Card: React.FunctionComponent<CardProps> = ({ title, paragraph }) => {
    
    return (<aside>
  <h2>{ title }</h2>
  <p>
    { paragraph }
  </p>
</aside>)

}

const el = <Card title="Welcome!" paragraph="To this example" />