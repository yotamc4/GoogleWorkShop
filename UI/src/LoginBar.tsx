import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import {getTheme, IImageProps, ImageFit, ITheme, Image,Stack, IStackTokens, StackItem, IStackItemStyles, IImageStyles, IStackStyles} from "@fluentui/react"
import IconButton from '@material-ui/core/IconButton';

const theme: ITheme =  getTheme();
const useStyles = makeStyles((theme) => ({
  title: {
    flexGrow: 1,
    alignItems:"left"
  },
}));

const serachFilterGapStackTokens: IStackTokens = {
    padding:65,
    };    

const imageProps: IImageProps = {
    src:"/Images/logo.PNG",
    imageFit: ImageFit.contain,
    };

export default function ButtonAppBar() {
  const classes = useStyles();


  const StackItemStyles: IStackItemStyles = {
    root: {
        position:"absolute",
        marginLeft:"43rem",
        marginTop:"-3rem",
        zIndex:300,
    },
  };
  
  return (
    <Stack  tokens={serachFilterGapStackTokens}>
        <StackItem styles={StackItemStyles}>
            <Image
                {...imageProps}
                width={200}
                height={140}
                />
        </StackItem>
      <AppBar style={{zIndex:1,marginLeft:"12rem",width:"75rem",height:"2.8rem",background:theme.palette.blueDark}} position="static">
        <Toolbar>
          <Typography style={{marginBottom:"1rem",marginRight:"15rem"}} variant="h6" className={classes.title}>
            Hello Guest!
          </Typography>
          <Button style={{marginBottom:"1rem"}} color="inherit">Login</Button>
        </Toolbar>
      </AppBar>
    </Stack>
  );
}
