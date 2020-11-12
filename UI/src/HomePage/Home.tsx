import React from 'react'; // importing FunctionComponent
import {Text, IStackTokens, Stack, Image, IImageProps, getTheme, ImageFit, IImageStyles, StackItem, IStackItemStyles, SearchBox, ISearchBoxStyles, IStackStyles, ITheme
} from "@fluentui/react"
import { ProductCard } from './ProductCard/ProductCard';
import {NavigationPane} from "./NavigationPane/NavigationPane"
const theme: ITheme =  getTheme();


export const Home: React.FunctionComponent = () => {
    const horizontalGapStackTokens: IStackTokens = {
        childrenGap: 20,
        };

    const categoryProductsGapStackTokens: IStackTokens = {
        childrenGap: 60,
        };
    

    const verticalGapStackTokens: IStackTokens = {
        padding:20,
        };    

    const serachFilterGapStackTokens: IStackTokens = {
        padding:20,
        };    

    const logoBarGapStackTokens: IStackTokens = {
        childrenGap: 0,
        };  
        const logoBarGapStackStyles: IStackStyles = {
            root: {
                marginRight:"32rem"
            },
          };

    const imageProps: IImageProps = {
        src:"/Images/logo.PNG",
        imageFit: ImageFit.contain,
        };

    
        const searchBoxStyles: Partial<ISearchBoxStyles> = { root: { width: 400 } };

    return (
    <Stack tokens={verticalGapStackTokens}>
        <Stack horizontal horizontalAlign="center" tokens={serachFilterGapStackTokens}> 
            <SearchBox styles={searchBoxStyles} placeholder="Search" underlined={true}/>
        </Stack>
        <Stack horizontal horizontalAlign="center" tokens={categoryProductsGapStackTokens}>
            <Stack tokens= {{childrenGap: 20}}>
                <StackItem>
                <div style={{marginRight:"35px", fontFamily: "Georgia", fontSize:"18px", fontWeight:600}}>Categories</div>
                </StackItem>
                <NavigationPane/>
            </Stack>
            <Stack tokens={horizontalGapStackTokens}>
                <Stack horizontal tokens={horizontalGapStackTokens}>
                    {test}
                    {test}
                    {test}
                </Stack>
                <Stack horizontal tokens={horizontalGapStackTokens}>
                    {test}
                    {test}
                    {test}
                </Stack>
                <Stack horizontal tokens={horizontalGapStackTokens}>
                    {test}
                    {test}
                    {test}
                </Stack>
            </Stack>
        </Stack>
    </Stack>
    );
}

const test = <ProductCard 
image="https://bstore.bezeq.co.il/media/20696/740-2-blue.jpg"
category="computers"
nameOfProduct = "Lenovo ThinkPad T480"
maxPrice = {4000}
creationDate = "11/11/2020"
expirationDate = "11/13/2020"
description = "Lenovo ThinkPad T480 is a Windows 10 laptop with a 14.00-inch display that has a resolution of 1920x1080 pixels."
potenialSuplliersCounter = {3}
unitsCounter = {95}
/>;

const el = <Home/>