import {
  FacebookIcon,
  FacebookShareButton,
  EmailShareButton,
  EmailIcon,
  LinkedinShareButton,
  LinkedinIcon,
  TwitterShareButton,
  TwitterIcon,
  WhatsappShareButton,
  WhatsappIcon,
} from "react-share";
import React from "react";
import { Stack } from "@fluentui/react";

export const ShareProductBar: React.FunctionComponent = () => {
  const productUrl: string = window.location.href;
  const quote: string = "Take a look at this group-buy on UniBuy";
  return (
    <Stack horizontal tokens={{ childrenGap: "0.5rem" }}>
      <FacebookShareButton url={productUrl} quote={quote}>
        <FacebookIcon size={32} round={true} />
      </FacebookShareButton>
      <WhatsappShareButton url={productUrl} title={quote}>
        <WhatsappIcon size={32} round={true} />
      </WhatsappShareButton>
      <TwitterShareButton url={productUrl} title={quote}>
        <TwitterIcon size={32} round={true} />
      </TwitterShareButton>
      <EmailShareButton url={productUrl} subject={quote}>
        <EmailIcon size={32} round={true} />
      </EmailShareButton>
      <LinkedinShareButton url={productUrl} summary={quote}>
        <LinkedinIcon size={32} round={true} />
      </LinkedinShareButton>
    </Stack>
  );
};
