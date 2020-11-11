import React from 'react'; // importing FunctionComponent
import { ProductCard } from './Card';



export const Home: React.FunctionComponent = () => {
    
    return (
    <div>
        <ProductCard 
        image="https://cdn.i-scmp.com/sites/default/files/d8/images/methode/2019/11/01/fca6da0e-fc77-11e9-acf9-cafedce87d15_image_hires_154757.jpg"
        category="computers"
        nameOfProduct = "Lenovo ThinkPad T480"
        maxPrice = {4000}
        creationDate = "11/11/2020"
        expirationDate = "11/13/2020"
        description = "Lenovo ThinkPad T480 is a Windows 10 laptop with a 14.00-inch display that has a resolution of 1920x1080 pixels. It is powered by a Core i7 processor and it comes with 12GB of RAM."
        potenialSuplliersCounter = {3}
        unitsCounter = {95}
        />
    </div>
    );
}

const el = <Home/>